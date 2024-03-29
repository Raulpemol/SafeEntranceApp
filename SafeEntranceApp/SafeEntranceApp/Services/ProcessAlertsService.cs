﻿using SafeEntranceApp.Common;
using SafeEntranceApp.Models;
using SafeEntranceApp.Services.Database;
using SafeEntranceApp.Services.Server;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SafeEntranceApp.Services
{
    public class ProcessAlertsService
    {
        private AlertsApiService alertsApiService;
        private VisitsService visitsService;
        private EnvironmentVariablesService environmentService;
        private PlacesApiService placesService;
        private CovidContactService contactService;
        private CovidAlertsService covidAlertsService;

        public ProcessAlertsService()
        {
            alertsApiService = new AlertsApiService();
            visitsService = new VisitsService();
            environmentService = new EnvironmentVariablesService();
            placesService = new PlacesApiService();
            contactService = new CovidContactService();
            covidAlertsService = new CovidAlertsService();
        }

        /*
         * Obtiene las variables de contagio del sistema y actualiza la base de datos de alertas del dispositivo
         */
        public async Task<int> Process()
        {
            int newAlerts = 0;
            DateTime syncDate = DateTime.Now;

            int daysAfterInfection = int.Parse((await environmentService.GetEnvironmentVariable(EnvironmentVariablesService.INFECTIVE_PERIOD)).Replace("\"", ""));
            int minutesForContact = int.Parse((await environmentService.GetEnvironmentVariable(EnvironmentVariablesService.TIME_TO_BE_DIRECT_CONTACT)).Replace("\"", ""));
            List<CovidAlert> ownAlerts = await covidAlertsService.GetAll();
            DateTime minDate = DateTime.Now.AddDays(-daysAfterInfection);

            var visits = await visitsService.GetAfterDate(minDate);

            if (visits.Count > 0)
            {
                DateTime lastSync = Preferences.Get(Constants.LAST_SYNC_PREFERENCE, DateTime.MinValue);
                List<CovidContact> contacts = await alertsApiService.GetPossibleContacts(visits, minutesForContact, lastSync, ownAlerts);

                if (contacts != null)
                {
                    contacts.ForEach(c => c.PlaceName = Task.Run(() => placesService.GetPlaceName(c.PlaceID)).Result.Replace("\"", ""));
                    contacts.ForEach(c => Task.Run(() => contactService.Save(c)));

                    newAlerts = contacts.Count;
                }
            }

            Preferences.Set(Constants.LAST_SYNC_PREFERENCE, syncDate);
            Preferences.Set(Constants.NEXT_SYNC_PREFERENCE, syncDate.AddHours(Constants.SYNC_FREQUENCIES[Preferences.Get(Constants.SYNC_PERIOD_PREFERENCE, 0)]));

            return newAlerts;
        }
    }
}
