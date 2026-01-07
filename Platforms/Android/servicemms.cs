using Android.Content;
using Android.Database;
using Android.Net;
using Microsoft.Maui.ApplicationModel;
using System.Collections.Generic;
using AUri = Android.Net.Uri;
using rappel.models;

namespace rappel.Platforms.Android
{
    public class MmsServiceAndroid : IMmsService
    {
        public List<MmsMessage> GetMmsMessages()
        {
            var messages = new List<MmsMessage>();
            var context = Platform.CurrentActivity?.ApplicationContext;
            if (context == null) return messages;

            // Récupérer les MMS
            var uri = AUri.Parse("content://mms/");
            string[] projection = { "_id" };

            using (ICursor cursor = context.ContentResolver.Query(uri, projection, null, null, null))
            {
                if (cursor == null) return messages;

                while (cursor.MoveToNext())
                {
                    string id = cursor.GetString(cursor.GetColumnIndex("_id"));
                    var mms = new MmsMessage { Id = id };

                    // --- Texte + Médias ---
                    var partUri = AUri.Parse("content://mms/part");
                    string selection = "mid=" + id;
                    using (ICursor partCursor = context.ContentResolver.Query(partUri, null, selection, null, null))
                    {
                        while (partCursor != null && partCursor.MoveToNext())
                        {
                            string contentType = partCursor.GetString(partCursor.GetColumnIndex("ct"));
                            if (contentType == "text/plain")
                            {
                                mms.Text = partCursor.GetString(partCursor.GetColumnIndex("text"));
                            }
                            else if (contentType.StartsWith("image/") || contentType.StartsWith("video/"))
                            {
                                string dataId = partCursor.GetString(partCursor.GetColumnIndex("_id"));
                                mms.MediaPaths.Add($"content://mms/part/{dataId}");
                            }
                        }
                    }

                    // --- Expéditeur ---
                    var addrUri = AUri.Parse($"content://mms/{id}/addr");
                    string[] addrProjection = { "address", "type" };
                    using (ICursor addrCursor = context.ContentResolver.Query(addrUri, addrProjection, null, null, null))
                    {
                        while (addrCursor != null && addrCursor.MoveToNext())
                        {
                            string address = addrCursor.GetString(addrCursor.GetColumnIndex("address"));
                            int type = addrCursor.GetInt(addrCursor.GetColumnIndex("type"));

                            if (type == 137) // 137 = expéditeur
                            {
                                mms.Sender = address;
                                break;
                            }
                        }
                    }

                    messages.Add(mms);
                }
            }
            return messages;
        }
    }
}
