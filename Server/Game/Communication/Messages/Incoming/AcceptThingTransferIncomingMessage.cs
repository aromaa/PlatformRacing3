using System;
using System.Collections.Generic;
using System.Text;
using Platform_Racing_3_Common.Block;
using Platform_Racing_3_Common.Database;
using Platform_Racing_3_Common.PrivateMessage;
using Platform_Racing_3_Server.Game.Client;
using Platform_Racing_3_Server.Game.Communication.Messages.Incoming.Json;

namespace Platform_Racing_3_Server.Game.Communication.Messages.Incoming
{
    internal class AcceptThingTransferIncomingMessage : MessageIncomingJson<JsonAcceptThingTransferIncomingMessage>
    {
        internal override void Handle(ClientSession session, JsonAcceptThingTransferIncomingMessage message)
        {
            if (session.IsGuest)
            {
                return;
            }
            
            //We could do this in sql too..... heh!
            PrivateMessageManager.GetPrivateMessageAsync(message.TransferId).ContinueWith((task) =>
            {
                IPrivateMessage pm = task.Result;
                if (pm is ThingTransferPrivateMessage thingTransferPm && thingTransferPm.ReceiverId == session.UserData.Id)
                {
                    if (thingTransferPm.ThingType == "block")
                    {
                        DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ExecuteNonQueryAsync($"WITH pm AS(DELETE FROM base.pms WHERE id = {message.TransferId} AND to_user_id = {session.UserData.Id} AND type = 'block' RETURNING id, to_user_id, from_user_id, type, sent_time), insertDeletedPm AS(INSERT INTO base.pms_deleted(id, to_user_id, from_user_id, type, sent_time) SELECT id, to_user_id, from_user_id, type, sent_time FROM pm), insertTitle AS (INSERT INTO base.blocks_titles(title, category, author_user_id) VALUES({message.Title}, {message.Category}, (SELECT to_user_id FROM pm)) ON CONFLICT(lower(title::text), lower(category::text), author_user_id) DO UPDATE SET title = blocks_titles.title RETURNING id) INSERT INTO base.blocks(id, version, description, image_data, settings) SELECT DISTINCT ON(b.id) (SELECT id FROM insertTitle), COALESCE(MAX(o.version) + 1, 1), {message.Description}, b.image_data, b.settings FROM base.blocks b LEFT JOIN base.blocks o ON o.id = (SELECT id FROM insertTitle) WHERE b.id = (SELECT block_id FROM base.transfers_block WHERE id = (SELECT id FROM pm)) GROUP BY b.id, b.version, b.image_data, b.settings ORDER BY b.id, b.version DESC RETURNING id")).Wait();
                    }
                    else if (thingTransferPm.ThingType == "level")
                    {
                        DatabaseConnection.NewAsyncConnection((dbConnection) => dbConnection.ExecuteNonQueryAsync($"WITH pm AS(DELETE FROM base.pms WHERE id = {message.TransferId} AND to_user_id = {session.UserData.Id} AND type = 'level' RETURNING id, to_user_id, from_user_id, type, sent_time), insertDeletedPm AS(INSERT INTO base.pms_deleted(id, to_user_id, from_user_id, type, sent_time) SELECT id, to_user_id, from_user_id, type, sent_time FROM pm), insertTitle AS (INSERT INTO base.levels_titles(title, author_user_id) VALUES({message.Title}, (SELECT to_user_id FROM pm)) ON CONFLICT(lower(title::text), author_user_id) DO UPDATE SET title = levels_titles.title RETURNING id) INSERT INTO base.levels(id, version, description, publish, song_id, mode, seconds, gravity, alien, sfchm, snow, wind, items, health, king_of_the_hat, bg_image, level_data) SELECT DISTINCT ON(l.id) (SELECT id FROM insertTitle), COALESCE(MAX(o.version) + 1, 1), {message.Description}, {message.Publish}, l.song_id, l.mode, l.seconds, l.gravity, l.alien, l.sfchm, l.snow, l.wind, l.items, l.health, l.king_of_the_hat, l.bg_image, l.level_data FROM base.levels l LEFT JOIN base.levels o ON o.id = (SELECT id FROM insertTitle) WHERE l.id = (SELECT level_id FROM base.transfers_level WHERE id = (SELECT id FROM pm)) GROUP BY l.id, l.version, l.song_id, l.mode, l.seconds, l.gravity, l.alien, l.sfchm, l.snow, l.wind, l.items, l.health, l.king_of_the_hat, l.bg_image, l.level_data ORDER BY l.id, l.version DESC RETURNING id")).Wait();
                    }
                }
            }).Wait();
        }
    }
}
