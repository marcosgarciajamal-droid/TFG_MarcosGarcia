using UnityEngine;
using System.Collections.Generic;


public class PlantController: MonoBehaviour {

  [Header("SENSORS")]
  public PrescenceSensor S1, S3, S12, S13, S14, SA_1, SA_2, SA_3, SB_1, SB_2, SB_3, SC_1, SC_2, SC_3;
  public OpticalSensor S2;
  public bool S8 => M2.Level0;
  public bool S9 => M2.Level1;
  public bool S10 => M2.Level2;
  public bool S11 => M2.Level3;

  public bool S4 => M3.Pos1;
  public bool S5 => M3.Pos2;
  public bool S6 => M3.Pos3;
  public bool S7 => M3.Pos4;

  public bool SV1 => M6.Level0;
  public bool SV2 => M6.Level1;
  public bool SV3 => M6.Level2;
  public bool SV4 => M6.Level3;

  public bool SH1 => M5.Pos1;
  public bool SH2 => M5.Pos2;

  [Header("ACTUATORS")]
  public MotorBelt M1, M8;
  public ASRSVertical M2, M6;

  public ASRSHorizontal M3, M5;
  public RollerConveyor M4, M7;
  public RollerConveyor M9_A, M9_B, M9_C;


  public int CountA, CountB, CountC, CEvacA, CEvacB, CEvacC;
  public LightSystem LS;
  [Header("CONTROL")]

  public bool B_Start = false; // Botón normalmente abierto, se CIERRA al PULSAR
  public bool B_Stop = true;  // Botón normalmente cerrado, se ABRE al PULSAR
  public bool B_Emergency = true; //BOTO NORMALMENTE CERRADO, SE ABRE EN EMERGENCIA

  public bool B_Pause = false; // Botón de pausa, se CIERRA al pulsar para activar la pausa

  public bool B_Reset = false; 

  [Header("ETAPES")]
  public int ETAPA_0 = 0;
  public int ETAPA_DT0 = 0;

  public int ETAPA_20 = 20;
  public int ETAPA_40 = 40;
  public int ETAPA_60 = 60;
  public int ETAPA_80 = 80;

  public int ETAPA_100 = 100; // M1
  public int ETAPA_120 = 120; // M4
  public int ETAPA_140 = 140; // M2
  public int ETAPA_160 = 160; // M3
  public int ETAPA_180 = 180; // M9_A
  public int ETAPA_200 = 200; // M9_B
  public int ETAPA_220 = 220; // M9_C
  public int ETAPA_240 = 240; // M7
  public int ETAPA_260 = 260; // M5
  public int ETAPA_280 = 280; // M6
  public int ETAPA_300 = 300; // M8
  public int ETAPA_500 = 500; // SELECTOR
  public int ETAPA_600 = 600; // EMERGENCIA, 600 indica que no hay emergencia,
  public int Selector = -1; // 0 para automático, 1 para manual
  private PieceShape Piece;
  Timer T0, T1, T2_A, T2_B, T2_C, TDT0;
  public PieceSpawner Spawner;
  [Header("CMD MANUAL")]
  public int CMD_M1 = 0;
  public int CMD_M8 = 0;
  public int CMD_M4 = 0;
  public int CMD_M7 = 0;
  public int CMD_M9_A = 0;
  public int CMD_M9_B = 0;
  public int CMD_M9_C = 0;
  public int CMD_M2 = -1; // nivel destino
  public int CMD_M3 = -1; // posición destino
  public int CMD_M5 = -1; // nivel destino
  public int CMD_M6 = -1; // posición destino
  public bool CI => !S1.isActivated && !S2.detected && !S3.isActivated && !S13.isActivated && !S14.isActivated && !S12.isActivated && S4 && S8 && SV1 && SH1; // Condición de Inicio
  public bool COND_EMERG_OK => ETAPA_600 == 600;
  public bool COND_AUTO => (ETAPA_500 == 501 || ETAPA_500 ==502) && COND_EMERG_OK && !B_Pause;
  public bool COND_MANUAL => ETAPA_500 == 503 || ETAPA_500 == 504;
  public bool CI_Actuators => ETAPA_100 == 100 && ETAPA_120 == 120 && ETAPA_140 == 140 && ETAPA_160 == 160 && ETAPA_180 == 180 && ETAPA_200 == 200 && ETAPA_220 == 220 && ETAPA_240 == 240 && ETAPA_260 == 260 && ETAPA_280 == 280 && ETAPA_300 == 300;

  Dictionary<string,bool> prevStates = new Dictionary<string, bool>();
  bool RisingEdge(string key, bool current)
  {
    bool prev = false;
    if (prevStates.ContainsKey(key)) prev = prevStates[key];

    prevStates[key] = current;
    return current && !prev;
  }


  void Start() {
    T1 = new Timer {preset = 1f}; 
    T2_A = new Timer {preset = 1f}; 
    T2_B = new Timer {preset = 2f}; 
    T2_C = new Timer {preset = 2.5f};
    T0 = new Timer {preset = 0.15f};
    TDT0 = new Timer {preset = 10f}; 
  }

  // Update is called once per frame
  void Update() {

    GRAFCET_EMERGENCIA();
    GRAFCET_MODO();
    Classification();
    Classification_DUALTWEEN();
    ShelfA();
    ShelfB();
    ShelfC();
    Evacuation();

    GRAFCET_M1();
    GRAFCET_M4();
    GRAFCET_M2();
    GRAFCET_M3();
    GRAFCET_M9_A();
    GRAFCET_M9_B();
    GRAFCET_M9_C();
    GRAFCET_M7();
    GRAFCET_M5();
    GRAFCET_M6();
    GRAFCET_M8();
  }
  
  void Classification() {
    switch (ETAPA_0) {
    case 0:
      prevStates["ETAPA_0_10"] = false;
      if (CI) ETAPA_0 = 1;
      break;
    case 1:
      if (COND_AUTO) ETAPA_0 = 10;
      break;
    case 10:
      if (RisingEdge("ETAPA_0_10", ETAPA_0 == 10)) Spawner.SpawnPiece();
      if (COND_AUTO && S1.isActivated) ETAPA_0 = 11;
      break;
    case 11:
      if (COND_AUTO && S2.detected) ETAPA_0 = 12;
      break;
    case 12:
      Piece = S2.detectedShape;
      if (COND_AUTO && S3.isActivated) ETAPA_0 = 13;
      break;
    case 13:
      T0.Update(true);
      if (T0.done) {
        T0.Update(false);
        ETAPA_0 = 14;
      }
      break;
    case 14:
      if (COND_AUTO && Piece == PieceShape.Square) ETAPA_0 = 15;
      else if (COND_AUTO && Piece == PieceShape.Cylinder) ETAPA_0 = 16;
      else if (COND_AUTO && Piece == PieceShape.Triangle) ETAPA_0 = 17;
      break;
    case 15:
      if (COND_AUTO && (ETAPA_20 == 28 || ETAPA_20 == 30 || ETAPA_20 == 32 || ETAPA_20 == 34)) ETAPA_0 = 18;
      break;
    case 16:
      if (COND_AUTO && (ETAPA_40 == 48 || ETAPA_40 == 50 || ETAPA_40 == 52 || ETAPA_40 == 54)) ETAPA_0 = 18;
      break;
    case 17:
      if (COND_AUTO && (ETAPA_60 == 68 || ETAPA_60 == 70 || ETAPA_60 == 72 || ETAPA_60 == 74)) ETAPA_0 = 18;
      break;
    case 18:
      if (COND_AUTO && ETAPA_140 == 142) ETAPA_0 = 19;
      break;

    case 19:
      if (COND_AUTO && ETAPA_160 == 162) ETAPA_0 = 0;
      break;
    }
  }

 void Classification_DUALTWEEN() {
    switch (ETAPA_DT0) {
    case 0:
      if (ETAPA_0 ==10) ETAPA_DT0 = 10;
      break;
    case 10:
      if (ETAPA_0 ==11) ETAPA_DT0 = 11;
      break;
    case 11:
      TDT0.Update(true);
      if (ETAPA_0 == 12) {
        ETAPA_DT0 = 12;
        TDT0.Update(false);
      }
      else if (TDT0.done && ETAPA_0 !=12) {
        TDT0.Update(false);
        ETAPA_DT0 = 21;
      }
      break;
    case 12:
      if (ETAPA_0 == 13) ETAPA_DT0 = 13;
      break;
    case 13:
      if (ETAPA_0 == 14) {ETAPA_DT0 = 14;
      }
      break;
    case 14:
      if (ETAPA_0 == 15) ETAPA_DT0 = 15;
      else if (ETAPA_0 == 16) ETAPA_DT0 = 16;
      else if (ETAPA_0 == 17) ETAPA_DT0 = 17;
      break;
    case 15:
      if (ETAPA_0 == 18) ETAPA_DT0 = 18;
      break;
    case 16:
      if (ETAPA_0 == 18) ETAPA_DT0 = 18;
      break;
    case 17:
      if (ETAPA_0 == 18) ETAPA_DT0 = 18;
      break;
    case 18:
      if (ETAPA_0 == 19) ETAPA_DT0 = 19;
      break;

    case 19:
      if (ETAPA_0 == 0) ETAPA_DT0 = 0;
      break;
    
    case 21:
      if (ETAPA_600 == 600 && ETAPA_0 == 0) ETAPA_DT0 = 0;
      break;
    }
  }
  void ShelfA() {
    switch (ETAPA_20) {
    case 20:
    
      prevStates["ETAPA_20_27"] = false;
      prevStates["ETAPA_20_34"] = false;

      if (COND_AUTO && ETAPA_0 == 15 && S3.isActivated) ETAPA_20 = 21;
      break;
    case 21:
      T2_A.Update(true);
      if (COND_AUTO && ETAPA_140 == 144 && T2_A.done) {
        ETAPA_20 = 22;
        T2_A.Update(false);
      }
      break;
    case 22:
      if (COND_AUTO && CountA % 3 == 0) ETAPA_20 = 23;
      else if (COND_AUTO && CountA % 3 == 1) ETAPA_20 = 24;
      else if (COND_AUTO && CountA % 3 == 2) ETAPA_20 = 25;
      break;
    case 23:
      if (COND_AUTO && ETAPA_160 == 164) ETAPA_20 = 26;
      break;
    case 24:
      if (ETAPA_160 == 166) ETAPA_20 = 26;
      break;
    case 25:
      if (COND_AUTO && ETAPA_160 == 168) ETAPA_20 = 26;
      break;
    case 26:
      T1.Update(true);
      if (COND_AUTO && !S3.isActivated && T1.done) {
        T1.Update(false);
        ETAPA_20 = 27;
      }
      break;
    case 27:
      if (RisingEdge("ETAPA_20_27", ETAPA_20 == 27)) CountA += 1;
      if (COND_AUTO && CountA % 3 != 0) ETAPA_20 = 28;
      else if (COND_AUTO && CountA == 3) ETAPA_20 = 29;
      else if (COND_AUTO && CountA == 6) ETAPA_20 = 31;

      else if (COND_AUTO && CountA == 9) ETAPA_20 = 33;
      break;
    case 28:
      if (COND_AUTO && ETAPA_0 == 0) ETAPA_20 = 20;
      break;

    case 29:
      if (COND_AUTO && SA_2.isActivated) ETAPA_20 = 30;
      break;
    case 30:
      if (COND_AUTO && ETAPA_0 == 0) ETAPA_20 = 20;
      break;
    case 31:
      if (COND_AUTO && SA_3.isActivated && SA_2.isActivated) ETAPA_20 = 32;
      break;
    case 32:
      if (COND_AUTO && ETAPA_0 == 0) ETAPA_20 = 20;
      break;
    case 33:
      if (COND_AUTO && ETAPA_80 == 88) ETAPA_20 = 34;
      break;
    case 34:
      if (RisingEdge("ETAPA_20_34", ETAPA_20 == 34))
      {
          CountA = 0;
          CEvacA += 1;
      };
      if (COND_AUTO && ETAPA_0 == 0) ETAPA_20 = 20;
      break;
    }
  }
  void ShelfB() {
    switch (ETAPA_40) {
    case 40:
      prevStates["ETAPA_40_47"] = false;
      prevStates["ETAPA_40_54"] = false;
      if (COND_AUTO && ETAPA_0 == 16 && S3.isActivated) ETAPA_40 = 41;
      break;
    case 41:
      T2_B.Update(true);
      if (COND_AUTO && ETAPA_140 == 146 && T2_B.done) {
        ETAPA_40 = 42;
        T2_B.Update(false);
      }
      break;
    case 42:
      if (COND_AUTO && CountB % 3 == 0) ETAPA_40 = 43;
      else if (COND_AUTO && CountB % 3 == 1) ETAPA_40 = 44;
      else if (COND_AUTO && CountB % 3 == 2) ETAPA_40 = 45;
      break;
    case 43:
      if (COND_AUTO && ETAPA_160 == 164) ETAPA_40 = 46;
      break;
    case 44:
      if (COND_AUTO && ETAPA_160 == 166) ETAPA_40 = 46;
      break;
    case 45:
      if (COND_AUTO && ETAPA_160 == 168) ETAPA_40 = 46;
      break;
    case 46:
      T1.Update(true);
      if (COND_AUTO && !S3.isActivated && T1.done) {
        T1.Update(false);
        ETAPA_40 = 47;
      }
      break;
    case 47:
      if (RisingEdge("ETAPA_40_47", ETAPA_40 == 47)) CountB += 1;
      if (COND_AUTO && CountB % 3 != 0) ETAPA_40 = 48;
      else if (COND_AUTO && CountB == 3) ETAPA_40 = 49;
      else if (COND_AUTO && CountB == 6) ETAPA_40 = 51;
      else if (COND_AUTO && CountB == 9) ETAPA_40 = 53;
      break;
    case 48:
      if (COND_AUTO && ETAPA_0 == 0) ETAPA_40 = 40;
      break;
    case 49:
      if (COND_AUTO && SB_2.isActivated) ETAPA_40 = 50;
      break;
    case 50:
      if (COND_AUTO && ETAPA_0 == 0) ETAPA_40 = 40;
      break;
    case 51:
      if (COND_AUTO && SB_2.isActivated && SB_3.isActivated) ETAPA_40 = 50;
      break;
    case 52:
      if (COND_AUTO && ETAPA_0 == 0) ETAPA_40 = 40;
      break;
    case 53: 
      if (COND_AUTO && ETAPA_80 == 88) ETAPA_40 = 54;
      break;
    case 54:
      if (RisingEdge("ETAPA_40_54", ETAPA_40 == 54))
      {
          CountB = 0;
          CEvacB += 1;
      };
      if (COND_AUTO && ETAPA_0 == 0) ETAPA_40 = 40;
      break;
    }
  }

  void ShelfC() {
    switch (ETAPA_60) {
    case 60:
       prevStates["ETAPA_60_67"] = false;
       prevStates["ETAPA_60_74"] = false;
      if (COND_AUTO && ETAPA_0 == 17 && S3.isActivated) ETAPA_60 = 61;
      break;
    case 61:
      T2_C.Update(true);
      if (COND_AUTO &&   ETAPA_140 == 148 && T2_C.done) {
        ETAPA_60 = 62;
        T2_C.Update(false);
      }
      break;
    case 62:
      if (COND_AUTO && CountC % 3 == 0) ETAPA_60 = 63;
      else if (COND_AUTO && CountC % 3 == 1) ETAPA_60 = 64;
      else ETAPA_60 = 65;
      break;
    case 63:
      if (COND_AUTO && ETAPA_160 == 164) ETAPA_60 = 66;
      break;
    case 64:
      if (COND_AUTO && ETAPA_160 == 166) ETAPA_60 = 66;
      break;
    case 65:
      if (COND_AUTO && ETAPA_160 == 168) ETAPA_60 = 66;
      break;
    case 66:
      T1.Update(true);
      if (COND_AUTO && !S3.isActivated && T1.done) {
        T1.Update(false);
        ETAPA_60 = 67;
      }
      break;
    case 67:
      if (RisingEdge("ETAPA_60_67", ETAPA_60 == 67)) CountC += 1;
      if (COND_AUTO && CountC % 3 != 0) ETAPA_60 = 68;
      else if (COND_AUTO && CountC == 3) ETAPA_60 = 69;
      else if (COND_AUTO && CountC == 6) ETAPA_60 = 71;
      else if (COND_AUTO && CountC == 9) ETAPA_60 = 73; 
      break;
    case 68:
      if (COND_AUTO &&  ETAPA_0 == 0) ETAPA_60 = 60;
      break;

    case 69: 
      if (COND_AUTO && SC_2.isActivated) ETAPA_60 = 70;
      break;
    case 70: 
      if (COND_AUTO && ETAPA_0 == 0) ETAPA_60 = 60;
      break;
    case 71:
      if (COND_AUTO && SC_3.isActivated) ETAPA_60 = 72;
      break;
    case 72:
      if (COND_AUTO && ETAPA_0 == 0) ETAPA_60 = 60;
      break;
    case 73:
      if (COND_AUTO && ETAPA_80 == 88) ETAPA_60 = 74;
      break;

    case 74: 
      if (RisingEdge("ETAPA_60_74", ETAPA_60 == 74))
      {
          CountC = 0;
          CEvacC += 1;
      };
      if (COND_AUTO && ETAPA_0 == 0) ETAPA_60 = 60;
      break;
    }
  }

  void Evacuation() {
    switch (ETAPA_80) {
    case 80:
      if (COND_AUTO && ETAPA_20 == 33) ETAPA_80 = 81;
      else if (COND_AUTO && ETAPA_20 != 33 && ETAPA_40 == 53) ETAPA_80 = 82;
      else if (COND_AUTO && ETAPA_20 != 33 && ETAPA_40 != 53 && ETAPA_60 == 73) ETAPA_80 = 83;
      break;
    case 81:
      if (COND_AUTO && ETAPA_280 == 284) ETAPA_80 = 84;
      break;
    case 82:
      if (COND_AUTO && ETAPA_280 == 286) ETAPA_80 = 84;
      break;
    case 83:
      if (COND_AUTO && ETAPA_280 == 288) ETAPA_80 = 84;
      break;
    case 84:
      if (COND_AUTO && Piece == PieceShape.Square) ETAPA_80 = 85;
      else if (COND_AUTO && Piece == PieceShape.Cylinder) ETAPA_80 = 86;
      else if (COND_AUTO && Piece == PieceShape.Triangle) ETAPA_80 = 87;
      break;

    case 85: // A
      if (COND_AUTO && S14.isActivated)  ETAPA_80 = 88;
      break;

    case 86: // B
      if (COND_AUTO && S14.isActivated) ETAPA_80 = 88;
      break;

    case 87: // C
      if (COND_AUTO && S14.isActivated) ETAPA_80 = 88;
      break;
    case 88:
      if (COND_AUTO && ETAPA_260 == 263) ETAPA_80 = 89;
      break;
    case 89:
      if (COND_AUTO && ETAPA_280 == 282) ETAPA_80 = 90;
      break;
    case 90:
      if (COND_AUTO && S12.isActivated && !S13.isActivated) ETAPA_80 = 91;
      break;
    case 91:
      if (COND_AUTO && !S12.isActivated) ETAPA_80 = 92;
      break;
    case 92:
      if (COND_AUTO && ETAPA_280 == 280 && ETAPA_260 == 260) ETAPA_80 = 80;
      break;
    }
  }

  void GRAFCET_M1() {
    switch (ETAPA_100) {
    case 100:
      M1.SetState(false);
      if (ETAPA_0 == 10 || ETAPA_0 == 12 || (COND_MANUAL && CMD_M1 == 1)) ETAPA_100 = 101;
      break;
    case 101: 
      M1.SetState(true);
      if (ETAPA_0 == 11 || ETAPA_0 == 13 || (COND_MANUAL && CMD_M1 == 0)) ETAPA_100 = 100;
      break;
    }
  }
  void GRAFCET_M4() {
    switch (ETAPA_120) {
    case 120: 
      M4.Stop();
      if (ETAPA_0 == 12 || ETAPA_0 == 13 || (COND_MANUAL && CMD_M4 == -1)) ETAPA_120 = 121;
      else if (ETAPA_20 == 26 || ETAPA_40 == 46 || ETAPA_60 == 66 || (COND_MANUAL && CMD_M4 == 1)) ETAPA_120 = 122;
      break;

    case 121: 
      M4.Active_and_forward();
      if (ETAPA_0 == 14 || (COND_MANUAL && CMD_M4 == 0)) ETAPA_120 = 120;
      break;

    case 122: 
      M4.Active_and_not_forward();
      if (ETAPA_20 == 27 || ETAPA_40 == 47 || ETAPA_60 == 67 || (COND_MANUAL && CMD_M4 == 0)) ETAPA_120 = 120;
      break;

    }
  }

  void GRAFCET_M2() {
    switch (ETAPA_140) {
    case 140:
      if (ETAPA_0 == 18 || (COND_MANUAL && CMD_M2 == 0 && !M3.IsMoving)) ETAPA_140 = 141;
      else if (ETAPA_20 == 21 || (COND_MANUAL && CMD_M2 == 1 && !M3.IsMoving)) ETAPA_140 = 143;
      else if (ETAPA_40 == 41 || (COND_MANUAL && CMD_M2 == 2 && !M3.IsMoving)) ETAPA_140 = 145;
      else if (ETAPA_60 == 61 || (COND_MANUAL && CMD_M2 == 3 && !M3.IsMoving)) ETAPA_140 = 147;
      break;
    case 141:
        CMD_M2 = -1;
        M2.MoveToLevel(0);
        if (S8)  ETAPA_140 = 142;
        break;
    case 142:
        if (ETAPA_0 == 19 || (COND_MANUAL && CMD_M2 != 0)) ETAPA_140 = 140;
        break;
    case 143:
      M2.MoveToLevel(1);
      if (S9) ETAPA_140 = 144;
      break;
    case 144:
      if (ETAPA_20 == 22 || (COND_MANUAL && CMD_M2 != 1)) ETAPA_140 = 140;
      break;
    case 145:
      M2.MoveToLevel(2);
      if (S10) ETAPA_140 = 146;
      break;
    case 146:
      if (ETAPA_40 == 42 || (COND_MANUAL && CMD_M2 != 2)) ETAPA_140 = 140;
      break;
    case 147:
      M2.MoveToLevel(3);
      if (S11) ETAPA_140 = 148;
      break;
    case 148:
      if (ETAPA_60 == 62 || (COND_MANUAL && CMD_M2 != 3)) ETAPA_140 = 140;
      break;
    }
  }

  void GRAFCET_M3() {
    switch (ETAPA_160) {
    case 160:
      if (ETAPA_0 == 19 || (COND_MANUAL && CMD_M3 == 0 && !M2.IsMoving)) ETAPA_160 = 161;
      else if (ETAPA_20 == 23 || ETAPA_40 == 43 || ETAPA_60 == 63 || (COND_MANUAL && CMD_M3 == 1 && !M2.IsMoving)) ETAPA_160 = 163;
      else if (ETAPA_20 == 24 || ETAPA_40 == 44 || ETAPA_60 == 64 || (COND_MANUAL && CMD_M3 == 2 && !M2.IsMoving)) ETAPA_160 = 165;
      else if (ETAPA_20 == 25 || ETAPA_40 == 45 || ETAPA_60 == 65 || (COND_MANUAL && CMD_M3 == 3 && !M2.IsMoving)) ETAPA_160 = 167;
      break;
    case 161:
        CMD_M3 = -1;
        M3.MoveToPos(1);
        if (S4) ETAPA_160 = 162;
        break;
    case 162:
        if (ETAPA_0 == 0 || (COND_MANUAL && CMD_M3 != 0)) ETAPA_160 = 160;
        break;
    case 163:
      M3.MoveToPos(2);
      if (S5) ETAPA_160 = 164;
      break;
    case 164:
      if (ETAPA_20 == 26 || ETAPA_40 == 46 || ETAPA_60 == 66 || (COND_MANUAL && CMD_M3 != 1)) ETAPA_160 = 160;
      break;
    case 165:
      M3.MoveToPos(3);
      if (S6) ETAPA_160 = 166;
      break;
    case 166:
      if (ETAPA_20 == 26 || ETAPA_40 == 46 || ETAPA_60 == 66 || (COND_MANUAL && CMD_M3 != 2)) ETAPA_160 = 160;
      break;
    case 167:
      M3.MoveToPos(4);
      if (S7) ETAPA_160 = 168;
      break;
    case 168:
      if (ETAPA_20 == 26 || ETAPA_40 == 46 || ETAPA_60 == 66 || (COND_MANUAL && CMD_M3 != 3)) ETAPA_160 = 160;
      break;
    }
  }

void GRAFCET_M5() {
    switch (ETAPA_260) {
    case 260:
        if (ETAPA_80 == 91 || (COND_MANUAL && CMD_M5 == 0 && !M6.IsMoving)) ETAPA_260 = 261;   // Ir a posición 1
        else if (ETAPA_80 == 88 || (COND_MANUAL && CMD_M5 == 1 && !M6.IsMoving)) ETAPA_260 = 262; // Ir a posición 2
        break;
    case 261:
        CMD_M5 = -1;
        M5.MoveToPos(1);
        if (SH1) ETAPA_260 = 260;
        break;
    case 262:
        M5.MoveToPos(2);
        if (SH2) ETAPA_260 = 263;
        break;
    case 263:
        if (ETAPA_80 == 89 || (COND_MANUAL && CMD_M5 != 1)) ETAPA_260 = 260;
        break;
    }
}
void GRAFCET_M6() {
    switch (ETAPA_280) {

    case 280:
        if ( ETAPA_80 == 89 || (COND_MANUAL && CMD_M6 == 0 && !M5.IsMoving)) ETAPA_280 = 281; // Nivel 0 (salida)
        else if ( ETAPA_80 == 81 || (COND_MANUAL && CMD_M6 == 1 && !M5.IsMoving)) ETAPA_280 = 283;
        else if (ETAPA_80 == 82 || (COND_MANUAL && CMD_M6 == 2 && !M5.IsMoving)) ETAPA_280 = 285;
        else if (ETAPA_80 == 83 || (COND_MANUAL && CMD_M6 == 3 && !M5.IsMoving)) ETAPA_280 = 287;
        break;
    case 281:
        M6.MoveToLevel(0);
        CMD_M6 = -1;
        if (SV1) ETAPA_280 = 282;
        break;
    case 282:
        if (ETAPA_80 == 90 || (COND_MANUAL && CMD_M6 != 0)) ETAPA_280 = 280;
        break;
    case 283:
        M6.MoveToLevel(1);
        if (SV2) ETAPA_280 = 284;
        break;
    case 284:
        if (ETAPA_80 == 84 || (COND_MANUAL && CMD_M6 != 1)) ETAPA_280 = 280;
        break;
    case 285:
        M6.MoveToLevel(2);
        if (SV3) ETAPA_280 = 286;
        break;
    case 286:
        if (ETAPA_80 == 84 || (COND_MANUAL && CMD_M6 != 2)) ETAPA_280 = 280;
        break;
    case 287:
        M6.MoveToLevel(3);
        if (SV4) ETAPA_280 = 288;
        break;
    case 288:
        if (ETAPA_80 == 84 || (COND_MANUAL && CMD_M6 != 3)) ETAPA_280 = 280;
        break;
    }
}

  void GRAFCET_M7() {
    switch (ETAPA_240) {
    case 240:
      M7.Stop();
      if (ETAPA_80 == 84 || ETAPA_80 == 85 || ETAPA_80 == 86 || ETAPA_80 == 87 || (COND_MANUAL && CMD_M7 == 1)) ETAPA_240 = 241;
      else if (ETAPA_80 == 90 || (COND_MANUAL && CMD_M7 == -1)) ETAPA_240 = 242;
      break;
    case 241: // Backward
      M7.Active_and_not_forward();
      if (ETAPA_80 == 88 || (COND_MANUAL && CMD_M7 == 0)) ETAPA_240 = 240;
      break;
    case 242: // Forward
      M7.Active_and_forward();
      if (ETAPA_80 == 91 || (COND_MANUAL && CMD_M7 == 0)) ETAPA_240 = 240;
      break;
    }
  }
  void GRAFCET_M8() {
    switch (ETAPA_300) {
    case 300:
      M8.SetState(false);
      if (ETAPA_80 == 90 || (COND_MANUAL && CMD_M8 == 1)) ETAPA_300 = 301;
      break;
    case 301:
      M8.SetState(true);
      if (ETAPA_80 == 92 || (COND_MANUAL && CMD_M8 == 0)) ETAPA_300 = 300;
      break;
    }
  }

  void GRAFCET_M9_A() {
    switch (ETAPA_180) {
    case 180:
      M9_A.Stop();
      if (ETAPA_20 == 29 || ETAPA_20 == 31 || ETAPA_80 == 85 || (COND_MANUAL && CMD_M9_A == 1)) ETAPA_180 = 181;
      break;
    case 181:
      M9_A.Active_and_not_forward();
      if (ETAPA_20 == 30 || ETAPA_20 == 32 || ETAPA_80 == 88 || (COND_MANUAL && CMD_M9_A == 0)) ETAPA_180 = 180;
      break;
    }
  }

  void GRAFCET_M9_B() {
    switch (ETAPA_200) {
    case 200:
      M9_B.Stop();
      if (ETAPA_40 == 49 || ETAPA_40 == 51 || ETAPA_80 == 86 || (COND_MANUAL && CMD_M9_B == 1)) ETAPA_200 = 201;
      break;
    case 201:
      M9_B.Active_and_not_forward();
      if (ETAPA_40 == 50 || ETAPA_40 == 52 || ETAPA_80 == 88 || (COND_MANUAL && CMD_M9_B == 0)) ETAPA_200 = 200;
      break;
    }
  }

  void GRAFCET_M9_C() {
    switch (ETAPA_220) {
    case 220:
      M9_C.Stop();
      if (ETAPA_60 == 69 || ETAPA_60 == 71 || ETAPA_80 == 87 || (COND_MANUAL && CMD_M9_C == 1)) ETAPA_220 = 221;
      break;
    case 221:
      M9_C.Active_and_not_forward();
      if (ETAPA_60 == 70 || ETAPA_60 == 72 || ETAPA_80 == 88 || (COND_MANUAL && CMD_M9_C == 0)) ETAPA_220 = 220;
      break;
    }
  }

void GRAFCET_MODO() {
  switch (ETAPA_500) {
    case 500: 
      LS.isOk = false; 
      LS.isManual = false;
      if (Selector == 0 && B_Start && ETAPA_600 == 600) ETAPA_500 = 501; // AUTO
      else if (Selector == 1 && B_Start && ETAPA_600 == 600) ETAPA_500 = 503;              
      break;
    case 501: 
      LS.isOk = true;
      if ((Selector != 0 && !B_Stop) || !B_Stop) ETAPA_500 = 502;
      break;
    case 502:
      if (ETAPA_0 == 0) ETAPA_500 = 500;
      break;
    case 503: 
      LS.isManual = true;
      if ((Selector != 1 && !B_Stop) || ETAPA_600 == 602) ETAPA_500 = 504;
      break;
    case 504:
      if (CI && CI_Actuators) ETAPA_500 = 500;
      break;
  }
}


void GRAFCET_EMERGENCIA() {
  switch (ETAPA_600) {
    case 600:
      LS.emergency = false;
      if (!B_Emergency || ETAPA_DT0 == 21) ETAPA_600 = 601;
      break;
    case 601:
      LS.isOk = false; 
      LS.emergency = true;
      ResetGrafcetsProcess(); 
      ETAPA_500 = 503;
      if (B_Emergency && B_Reset) ETAPA_600 = 602; 
      break;
    case 602: // Reset emergencia
      if (ETAPA_500 == 500) ETAPA_600 = 600;
      break;
  }
}


void ResetGrafcetsProcess() {
    ETAPA_0 = 0;
    ETAPA_20 = 20;
    ETAPA_40 = 40;
    ETAPA_60 = 60;
    ETAPA_80 = 80;

    ETAPA_DT0 = 0;
  }
}


