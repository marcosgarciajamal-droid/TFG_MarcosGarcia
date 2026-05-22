using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Text;

public class MQTTprova : MonoBehaviour
{
    MqttClient client;

    string brokerAddress = "127.0.0.1";

    void Start()
    {
        client = new MqttClient(brokerAddress);

        client.MqttMsgPublishReceived += OnMessageReceived;

        client.Connect("UnityClient");

        client.Subscribe(new string[] { "factory/motor" },
                         new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

        Debug.Log("MQTT conectado");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PublishSensorValue();
        }
    }

    void PublishSensorValue()
    {
        string topic = "factory/sensor";
        string message = Random.Range(0,100).ToString();

        client.Publish(topic, Encoding.UTF8.GetBytes(message));

        Debug.Log("Publicado: " + message);
    }

    void OnMessageReceived(object sender, MqttMsgPublishEventArgs e)
    {
        string msg = Encoding.UTF8.GetString(e.Message);

        Debug.Log("Mensaje recibido: " + msg);

        if(msg == "ON")
        {
            Debug.Log("Motor activado");
        }
    }
}