﻿using Supabase;
using System.Configuration;
using System.Data;
using System.Windows;
using static Supabase.Gotrue.Constants;

namespace ArchOp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    
    public partial class App : Application
    {
        public static Client SupabaseClient { get; private set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var url = "https://bunlwthqohijzerjlqnc.supabase.co";
            var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImJ1bmx3dGhxb2hpanplcmpscW5jIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzQ2MTA5NTYsImV4cCI6MjA1MDE4Njk1Nn0.FJvlcLqMbVTzxi8FLJQygHYf3MFBJaHm8TfqPhP0FEA";

            var options = new Supabase.SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            SupabaseClient = new Client(url, key, options);
            await SupabaseClient.InitializeAsync();

            SupabaseClient.Auth.AddStateChangedListener((sender, changed) =>
            {
                if (sender.CurrentSession == null && changed == AuthState.SignedIn)
                    LoadHome();

            });
        }
        
        private void LoadHome()
        {
            Current.Windows.OfType<LoginWindow>().FirstOrDefault()?.LoadHome();
        }
       
    }

}
