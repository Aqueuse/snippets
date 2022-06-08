using System;
using System.Collections.Generic;

// Class to parse Twitch IRC message with special capabilities

// Sample message :
//
// @badge - info = subscriber / 8;
// badges = broadcaster / 1,subscriber / 0,premium / 1;
// client - nonce = 613504e2dc98c210973e64b6d6e56d9d;
// color =#8A2BE2;
// display-name=Aqueuse;
// emotes=;
// first-msg=0;
// flags=;
// id=4893b988-1cf9-4f42-a28b-7a77011600eb;
// mod=0;
// room-id=462818643;subscriber=1;
// tmi-sent-ts=1654694579465;
// turbo=0;
// user-id=462818643;
// user-type= :aqueuse!aqueuse@aqueuse.tmi.twitch.tv PRIVMSG #aqueuse :plop

public class SC_TmiMessageParser {
    public static Dictionary<string, string> parsedMessage(string rawMessage) {
        Dictionary<string, string> parsedMessage = new Dictionary<string, string>();

        string[] dirtyMessage = rawMessage.Split("!");

        if (dirtyMessage.Length < 2) {
            Console.WriteLine("incorrect message, can't parsed it");
            return parsedMessage;
        }
        else {
            string[] messageParameters = dirtyMessage[0].Split(";");

            if (messageParameters.Length > 0) {
                for (int i = 0; i < messageParameters.Length; i++) {
                    string[] parameter = messageParameters[i].Split("=");

                    if (parameter.Length == 1) {
                        parsedMessage.Add(parameter[0].Replace(" ", ""), null);
                    }
                    else {
                        parsedMessage.Add(parameter[0].Replace(" ", ""), parameter[1].Trim());
                    }
                }
            }
        }

        Console.WriteLine(rawMessage.Substring(rawMessage.IndexOf("!")));
        parsedMessage.Add("IRC-message", rawMessage.Substring(rawMessage.IndexOf("!")));
        return parsedMessage;
    }

    // !aqueuse@aqueuse.tmi.twitch.tv PRIVMSG #aqueuse :plop
    public static Dictionary<string, string> parsedPRIVMSG(string rawPRIVMSG) {
        Dictionary <string, string> parsedPRIVMSG = new Dictionary<string, string>();

        string username = rawPRIVMSG.Substring(1, rawPRIVMSG.IndexOf("@") - 1);
        string message = rawPRIVMSG.Substring(rawPRIVMSG.IndexOf(":") + 1);

        parsedPRIVMSG.Add("username", username);
        parsedPRIVMSG.Add("message", message);

        return parsedPRIVMSG;
    }
}