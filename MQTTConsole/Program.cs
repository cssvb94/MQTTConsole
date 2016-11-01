using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;


namespace MQTTConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting MQTT client...");
            MqttClient client = new MqttClient("localhost", 1883, false, MqttSslProtocols.None, null, null);

            client.ConnectionClosed += client_ConnectionClosed;
            client.MqttMsgSubscribed += client_MqttMsgSubscribed;
            client.MqttMsgUnsubscribed += client_MqttMsgUnsubscribed;
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;

            Console.WriteLine("Connecting...");
            byte id = client.Connect("Bazinga");

            client.Subscribe(new string[] { "/IoTmanager" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { "/IoTmanager/+/+/status" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { "/IoTmanager/+/response" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            client.Subscribe(new string[] { "/IoTmanager/+/config" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

            Console.WriteLine("Subscribed to topics");

            Console.WriteLine("Connection id: {0}", id);

            //client.Publish("/IoTManager", Encoding.UTF8.GetBytes("HELLO"));

            Console.ReadLine();

            client.Disconnect();
        }

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine("Message received: {0}", Encoding.UTF8.GetString(e.Message));
        }

        static void client_MqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
        {
            Console.WriteLine("Unsubscribed from: {0}", e.MessageId);
        }

        static void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Console.WriteLine("Subscribed to: {0}", e.MessageId);
        }

        static void client_ConnectionClosed(object sender, EventArgs e)
        {
            Console.WriteLine("Connection closed!");
        }
    }
}
