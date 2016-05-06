using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Com.Payleven.Payment.Api;
using System;
using Java.Util;
using System.Collections.Generic;

namespace Sample
{
	[Activity]
	public class ResultActivity : Activity
	{

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			SetContentView (Resource.Layout.Result);

			TextView paymentResultView = FindViewById<TextView> (Resource.Id.payment_result);

			Intent i = new Intent();
			if (this.Intent.HasExtra("result")) {
				i.PutExtras(this.Intent.Extras.GetBundle("result"));
			}
			int requestCode = this.Intent.GetIntExtra("request_code", 0);

			PaylevenApi.HandleIntent (requestCode, i, new PaylevenCallback (paymentResultView));
		}

		class PaylevenCallback : Java.Lang.Object, IPaylevenResponseListener
		{
			TextView paymentDataView;

			public PaylevenCallback (TextView paymentDataView)
			{
				this.paymentDataView = paymentDataView;
			}

			public void OnNoPaylevenResponse (Intent data)
			{
				
			}

			public void OnOpenSalesHistoryFinished (ApiSalesHistoryCompletedStatus status)
			{

			}

			public void OnOpenTransactionDetailsFinished (string p0, IDictionary<string, string> p1, ApiTransactionDetailsCompletedStatus p2)
			{

			}

			public void OnPaymentFinished (string orderId, TransactionRequest originalRequest, IDictionary<String, String> result, ApiPaymentCompletedStatus status)
			{
				//handle payment finished
				string textResult = "Order ID: " + orderId + "\n" + "Status: " + status + "\n";


				foreach(KeyValuePair<string, string> entry in result)
				{
					textResult = textResult + entry.Key + ": " + entry.Value + "\n";
				}

				this.paymentDataView.Text = textResult;
			}
		}
	}
}

