using System.Collections.Generic;
using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Text;

public class MQTTManager : MonoBehaviour
{
    MqttClient client;
    string brokerAddress = "127.0.0.1";
    public PlantController plant;
    int shapeValue = 0;
    Dictionary<string, int> lastValues = new Dictionary<string, int>();
    void Start()
    {
        client = new MqttClient(brokerAddress);
        client.MqttMsgPublishReceived += OnMessageReceived;
        client.Connect("UnityClient");
        client.Subscribe(new string[] { "plant/cmd/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
    }
void Update()
{
    PublishSensors();
    PublishActuators();
    PublishCounters();
    PublishStatus();
}
    void PublishIfChanged(string topic, int value)
{
    if (!lastValues.ContainsKey(topic) || lastValues[topic] != value)
    {
        lastValues[topic] = value;
        client.Publish(topic, Encoding.UTF8.GetBytes(value.ToString()));
    }
}

void PublishSensors()
{
    PublishIfChanged("plant/sensors/S1", plant.S1.isActivated ? 1 : 0);
    PublishIfChanged("plant/sensors/S3", plant.S3.isActivated ? 1 : 0);
    PublishIfChanged("plant/sensors/S12", plant.S12.isActivated ? 1 : 0);
    PublishIfChanged("plant/sensors/S13", plant.S13.isActivated ? 1 : 0);
    PublishIfChanged("plant/sensors/S14", plant.S14.isActivated ? 1 : 0);

    if (plant.S2.detected)
    {
        switch (plant.S2.detectedShape)
        {
            case PieceShape.Square: shapeValue = 1; break;
            case PieceShape.Cylinder: shapeValue = 2; break;
            case PieceShape.Triangle: shapeValue = 3; break;
        }
    }

    PublishIfChanged("plant/sensors/S2", shapeValue);
    PublishIfChanged("plant/sensors/S8", plant.S8 ? 1 : 0);
    PublishIfChanged("plant/sensors/S9", plant.S9 ? 1 : 0);
    PublishIfChanged("plant/sensors/S10", plant.S10 ? 1 : 0);
    PublishIfChanged("plant/sensors/S11", plant.S11 ? 1 : 0);

    PublishIfChanged("plant/sensors/S4", plant.S4 ? 1 : 0);
    PublishIfChanged("plant/sensors/S5", plant.S5 ? 1 : 0);
    PublishIfChanged("plant/sensors/S6", plant.S6 ? 1 : 0);
    PublishIfChanged("plant/sensors/S7", plant.S7 ? 1 : 0);

    PublishIfChanged("plant/sensors/SH1", plant.SH1 ? 1 : 0);
    PublishIfChanged("plant/sensors/SH2", plant.SH2 ? 1 : 0);

    PublishIfChanged("plant/sensors/SV1", plant.SV1 ? 1 : 0);
    PublishIfChanged("plant/sensors/SV2", plant.SV2 ? 1 : 0);
    PublishIfChanged("plant/sensors/SV3", plant.SV3 ? 1 : 0);
    PublishIfChanged("plant/sensors/SV4", plant.SV4 ? 1 : 0);

    PublishIfChanged("plant/sensors/SA_1", plant.SA_1.isActivated ? 1 : 0);
    PublishIfChanged("plant/sensors/SA_2", plant.SA_2.isActivated ? 1 : 0);
    PublishIfChanged("plant/sensors/SA_3", plant.SA_3.isActivated ? 1 : 0);

    PublishIfChanged("plant/sensors/SB_1", plant.SB_1.isActivated ? 1 : 0);
    PublishIfChanged("plant/sensors/SB_2", plant.SB_2.isActivated ? 1 : 0);
    PublishIfChanged("plant/sensors/SB_3", plant.SB_3.isActivated ? 1 : 0);
    PublishIfChanged("plant/sensors/SC_1", plant.SC_1.isActivated ? 1 : 0);
    PublishIfChanged("plant/sensors/SC_2", plant.SC_2.isActivated ? 1 : 0);
    PublishIfChanged("plant/sensors/SC_3", plant.SC_3.isActivated ? 1 : 0);
    }
    void PublishCounters()
{
    PublishIfChanged("plant/counters/C1", plant.CountA);
    PublishIfChanged("plant/counters/C2", plant.CountB);
    PublishIfChanged("plant/counters/C3", plant.CountC);
    PublishIfChanged("plant/counters/C_EvacA", plant.CEvacA);
    PublishIfChanged("plant/counters/C_EvacB", plant.CEvacB);
    PublishIfChanged("plant/counters/C_EvacC", plant.CEvacC);
}
void PublishActuators()
    {
    PublishIfChanged("plant/actuators/M1", plant.M1.beltEnabled ? 1 : 0);
    PublishIfChanged("plant/actuators/M2", plant.M2.IsMoving ? 1 : 0);
    PublishIfChanged("plant/actuators/M3", plant.M3.IsMoving ? 1 : 0);
    PublishIfChanged("plant/actuators/M4", plant.M4.state);
    PublishIfChanged("plant/actuators/M5", plant.M5.IsMoving ? 1 : 0);
    PublishIfChanged("plant/actuators/M6", plant.M6.IsMoving ? 1 : 0);
    PublishIfChanged("plant/actuators/M7", plant.M7.state);
    PublishIfChanged("plant/actuators/M8", plant.M8.beltEnabled ? 1 : 0);
    PublishIfChanged("plant/actuators/M9_A", plant.M9_A.state);
    PublishIfChanged("plant/actuators/M9_B", plant.M9_B.state);
    PublishIfChanged("plant/actuators/M9_C", plant.M9_C.state);  
    }

void PublishStatus()
{
    int emergency = (plant.ETAPA_600 == 600) ? 0 : 1;
    PublishIfChanged("plant/alarms/emergency_active", emergency);
    int status = 0;
    switch (plant.ETAPA_500){
        case 500: status = 0; break; // STOPPED
        case 501: status = 1; break; // AUTO RUNNING
        case 502: status = 1; break; // AUTO STOPPING
        case 503: status = 2; break; // MANUAL RUNNING
        case 504: status = 2; break; // MANUAL STOPPING
    }
    PublishIfChanged("plant/status/actuation_mode", status);

}

void OnMessageReceived(object sender, MqttMsgPublishEventArgs e)
{
    string topic = e.Topic;
    int value = int.Parse(Encoding.UTF8.GetString(e.Message));

    // GLOBAL

    if (topic == "plant/cmd/start" && value == 1) plant.B_Start = true;
    if (topic == "plant/cmd/start" && value == 0) plant.B_Start = false;

    if (topic == "plant/cmd/stop" && value == 1) plant.B_Stop = false;
    if (topic == "plant/cmd/stop" && value == 0) plant.B_Stop = true;

    if (topic == "plant/cmd/pause" && value == 1) plant.B_Pause = true;
    if (topic == "plant/cmd/pause" && value == 0) plant.B_Pause = false;

    if (topic == "plant/cmd/emergency" && value == 1) plant.B_Emergency = false;
    if (topic == "plant/cmd/emergency" && value == 0) plant.B_Emergency = true;

    if (topic == "plant/cmd/reset" && value == 1) plant.B_Reset = true;
    if (topic == "plant/cmd/reset" && value == 0) plant.B_Reset = false;
 
    if (topic == "plant/cmd/selector") plant.Selector = value;
    // CINTA PRINCIPAL
    if (topic == "plant/cmd/manual/M1") plant.CMD_M1 = value;
    //TRANSELEVADOR 1
    if (topic == "plant/cmd/manual/M2") plant.CMD_M2 = value;
    if (topic == "plant/cmd/manual/M3") plant.CMD_M3 = value;
    if (topic == "plant/cmd/manual/M4") plant.CMD_M4 = value;
    // TRANSELEVADOR 2
    if (topic == "plant/cmd/manual/M5") plant.CMD_M5 = value;
    if (topic == "plant/cmd/manual/M6") plant.CMD_M6 = value;
    if (topic == "plant/cmd/manual/M7") plant.CMD_M7 = value;
    // estanterias
    if (topic == "plant/cmd/manual/M9_A") plant.CMD_M9_A = value;
    if (topic == "plant/cmd/manual/M9_B") plant.CMD_M9_B = value;    
    if (topic == "plant/cmd/manual/M9_C") plant.CMD_M9_C = value;
    // CINTA EVAC
    if (topic == "plant/cmd/manual/M8")  plant.CMD_M8 = value;
    }
}
