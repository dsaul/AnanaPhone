using Amazon.S3;
using Amazon.S3.Model;
using AnanaPhone;
using AnanaPhone.Calls;
using AnanaPhone.VoiceMail;
using DanSaul.SharedCode.StandardizedEnvironmentVariables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Serilog;

namespace ASPNetServer
{
	[Route("api/[controller]")]
	[ApiController]
	public class MessageAudioController : ControllerBase
	{
		VoiceMailManager VMM { get; init; }
		AmazonS3Client S3Client { get; init; }

		public MessageAudioController(VoiceMailManager _VMM, AmazonS3Client _S3Client)
        {
			VMM = _VMM;
			S3Client = _S3Client;
		}

        [HttpGet("{id}")]
		public async Task Get([FromRoute] string id)
		{
			string[] ids = new string[] { id };


			List<VoiceMailMessageRow> messages = VMM.ForIds(ids);
			if (!messages.Any())
			{
				Response.StatusCode = 404;
				await Response.Body.FlushAsync();
				return;
			}

			VoiceMailMessageRow first = messages.First();

			string bucket = Env.VMAIL_S3_BUCKET;
			string key = $"{first.Id}/recording.wav";

			Log.Information("[{Class}.{Method}()] S3 Fetch bucket:{bucket} key:{key}",
				GetType().Name,
				System.Reflection.MethodBase.GetCurrentMethod()?.Name,
				bucket,
				key
			);

			GetObjectRequest request = new()
			{
				BucketName = bucket,
				Key = key,
			};

			GetObjectResponse s3response = await S3Client.GetObjectAsync(request);

			using Stream s3Stream = s3response.ResponseStream;

			Response.StatusCode = 200;
			Response.Headers.Add(HeaderNames.ContentType, s3response.Headers.ContentType);

			await s3Stream.CopyToAsync(Response.Body);
			await Response.Body.FlushAsync();
		}

	}
}
