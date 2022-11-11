using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace soufGame.Model.NetworkModel;

public class NetworkChatUser
{
    public string username { get; set; } = "INVALID";
    public int score { get; set; } = -1;
    public int messagesSent { get; set; } = -1;

    public NetworkChatUser(string username = "", int score = 0, int messagesSent = 0)
    {
        this.username = username;
        this.score = score;
        this.messagesSent = messagesSent;
    }

    public NetworkChatUser() { }


}