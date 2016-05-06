using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Com.Payleven.Payment.Api;
using System;
using Java.Util;

namespace Sample
{
	[Activity (Label = "PointPayAPI-Xamarin", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		string apiKey = "6838f633caa8472285233e996152d29f";

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			//Set up Point Pay API
			PaylevenApi.Configure(apiKey);

			// Get our views from the layout resource
			Button startPaymentButton = FindViewById<Button> (Resource.Id.button_start_payment);
			EditText descriptionEditText = FindViewById<EditText> (Resource.Id.editText_description);
			EditText amountEditText = FindViewById<EditText> (Resource.Id.editText_amount);
			RadioGroup currencyRadioGroup = FindViewById<RadioGroup> (Resource.Id.currency_radiogroup);

			// attach event to the button
			startPaymentButton.Click += delegate {
				try {
					//start payment
					int amount = Int32.Parse(amountEditText.Text);
					string description = descriptionEditText.Text;

					RadioButton checkedButton = FindViewById<RadioButton> (currencyRadioGroup.CheckedRadioButtonId);
					Currency currency = Currency.GetInstance(checkedButton.Text);

					TransactionRequestBuilder builder = new TransactionRequestBuilder(amount, currency);
					builder.SetDescription(description);

					TransactionRequest transactionRequest = builder.CreateTransactionRequest();


					Guid g = Guid.NewGuid();
					string orderId = Convert.ToBase64String(g.ToByteArray());
					orderId = orderId.Replace("=","");
					orderId = orderId.Replace("+","");

					PaylevenApi.InitiatePayment(this, orderId, transactionRequest);
					
				} catch (Exception e) {
					//handle error
				}
			};
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			//handle response in ResultActivity
			Intent i = new Intent(this, typeof(ResultActivity));
			if (data != null) {
				i.PutExtra("result", data.Extras);
			}
			i.PutExtra("request_code", requestCode);
			this.StartActivity(i);
		}
	}
}


