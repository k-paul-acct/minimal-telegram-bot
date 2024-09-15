using Telegram.Bot.Requests;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

namespace MinimalTelegramBot.Client;

internal static class WebhookResponseAvailabilityInfo
{
    public static bool IsWebhookResponseAvailable(this IRequest request)
    {
        return request switch
        {
            // Callback query actions.
            AnswerCallbackQueryRequest => true,

            // Edit actions.
            EditMessageCaptionRequest => true,
            EditMessageReplyMarkupRequest => true,
            EditMessageTextRequest => true,

            // Send actions.
            SendAnimationRequest sendAnimationRequest => sendAnimationRequest.Animation is not InputFileStream &&
                                                         sendAnimationRequest.Thumbnail is not InputFileStream,
            SendAudioRequest sendAudioRequest => sendAudioRequest.Audio is not InputFileStream &&
                                                 sendAudioRequest.Thumbnail is not InputFileStream,
            SendContactRequest => true,
            SendDiceRequest => true,
            SendDocumentRequest sendDocumentRequest => sendDocumentRequest.Document is not InputFileStream &&
                                                       sendDocumentRequest.Thumbnail is not InputFileStream,
            SendGameRequest => true,
            SendLocationRequest => true,
            SendMessageRequest => true,
            SendPhotoRequest sendPhotoRequest => sendPhotoRequest.Photo is not InputFileStream,
            SendStickerRequest sendStickerRequest => sendStickerRequest.Sticker is not InputFileStream,
            SendVenueRequest => true,
            SendVideoNoteRequest sendVideoNoteRequest => sendVideoNoteRequest.VideoNote is not InputFileStream &&
                                                         sendVideoNoteRequest.Thumbnail is not InputFileStream,
            SendVideoRequest sendVideoRequest => sendVideoRequest.Video is not InputFileStream &&
                                                 sendVideoRequest.Thumbnail is not InputFileStream,
            SendVoiceRequest sendVoiceRequest => sendVoiceRequest.Voice is not InputFileStream,

            // All the others request types are not available.
            _ => false,
        };
    }
}
