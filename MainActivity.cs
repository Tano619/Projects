using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using System;
using System.Collections.Generic;

namespace MyFirstApp
{
	[Activity (Label = "Phoneword", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		//int count = 1;
		static readonly List<string> phoneNumbers = new List<string>();
		EditText phoneNumberText;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it

			 phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
			var translateButton = FindViewById<Button>(Resource.Id.TranslateButton);
			var callButton = FindViewById<Button>(Resource.Id.CallButton);

			// Disable the "Call" button
			callButton.Enabled = false;

			// Add code to translate number
			string translatedNumber = string.Empty;

			translateButton.Click += (object sender, EventArgs e) =>
			{
				// Translate user's alphanumeric phone number to numeric
				translatedNumber = MyFirstApp.PhoneTranslator.ToNumber(phoneNumberText.Text);//PhonewordTranslator.ToNumber(phoneNumberText.Text);
				if (String.IsNullOrWhiteSpace(translatedNumber))
				{
					callButton.Text = "Call";
					callButton.Enabled = false;
				}
				else
				{
					callButton.Text = "Call" + translatedNumber;
					callButton.Enabled = true;
				}
			};

			Button callHistoryButton = FindViewById<Button> (Resource.Id.CallHistoryButton);

			callHistoryButton.Click += (object sender, EventArgs e) => 
			{
				var intent = new Intent(this , typeof(CallHistoryActivity));
				intent.PutStringArrayListExtra("phone_numbers", phoneNumbers);
				StartActivity(intent);
			}; 

			callButton.Click += (object sender, EventArgs e) =>
			{
				// add dialed number to list of called numbers.
				phoneNumbers.Add(translatedNumber);
				// enable the Call History button
				callHistoryButton.Enabled = true;
				// On "Call" button click, try to dial phone number.
				var callDialog = new AlertDialog.Builder(this);
				callDialog.SetMessage("Call " + translatedNumber + "?");
				callDialog.SetNeutralButton("Call", delegate {
					// Create intent to dial phone
					var callIntent = new Intent(Intent.ActionCall);
					callIntent.SetData(Android.Net.Uri.Parse("tel:" + translatedNumber));
					StartActivity(callIntent);
				});
				callDialog.SetNegativeButton("Cancel", delegate { });

				// Show the alert dialog to the user and wait for response.
				callDialog.Show();

			};

		}

	}
}
