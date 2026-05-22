using UnityEngine;

public class PlantController : MonoBehaviour
{

    [Header("SENSORS")]
    public PrescenceSensor S1, S3, S12, S13;
    
    public OpticalSensor S2;
    public PrescenceSensor SA_1, SA_2, SA_3, SB_1, SB_2, SB_3, SC_1, SC_2, SC_3; // Sensores estantería 

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

    [Header ("COUNTERS")]
    public Counter C_A, C_B, C_C, C_T;

    public CounterEvac C_Evac;
    
    [Header("CONTROL")]
    public bool CI = true;

    public bool B_Start = false;

    [Header("ETAPES")]
    public int ETAPA_0 = 0;

    public int ETAPA_20 = 20;
    public int ETAPA_40 = 40;
    public int ETAPA_60 = 60;
    public int ETAPA_80 = 80;

    private PieceShape Piece;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Timer T0, T1, T2_A, T2_B, T2_C;
    

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

    void Start()
    {
        T1 = new Timer {preset = 2f}; //DEJAR PIEZA EN ESTANTERIA
        T2_A = new Timer {preset = 1f}; //DIFERENCIA ENTRE MOVER T1 VERTICAL Y HORIZONTAL, ESTANT A
        T2_B = new Timer {preset = 2f}; //DIFERENCIA ENTRE MOVER T1 VERTICAL Y HORIZONTAL, ESTANT B
        T2_C = new Timer {preset = 3f}; //DIFERENCIA ENTRE MOVER T1 VERTICAL Y HORIZONTAL, ESTANT C

        T0 = new Timer {preset = 0.15f}; //DEJAR PIEZA EN PLATAFORMA DE T1 

    }

    // Update is called once per frame
    void Update()
    {

        Classification();
        ShelfA();
        ShelfB();
        ShelfC();
        Evacuation();
            if (Input.GetKeyDown(KeyCode.Alpha1))
        Time.timeScale = 1f;

    if (Input.GetKeyDown(KeyCode.Alpha2))
        Time.timeScale = 2f;

    if (Input.GetKeyDown(KeyCode.Alpha3))
        Time.timeScale = 5f;
    }

void Classification()
    {
        switch (ETAPA_0)
        {
            case 0:
                if (CI && B_Start) ETAPA_0 = 10;
                break;
            case 10:
                M1.SetState(true);
                if (S1.isActivated) ETAPA_0 = 11;
                break;
            case 11:
                M1.SetState(false);
                if (S2.detected) ETAPA_0 = 12;
                break;
            case 12:
                Piece = S2.detectedShape;
                M1.SetState(true);
                M4.Active_and_forward();
                if (S3.isActivated)  ETAPA_0 = 13;
                break;
            case 13: 
                M1.SetState(false);

                T0.Update(true);
                if (T0.done) {T0.Update(false); ETAPA_0 = 14;}
                break;
            case 14:
                M4.Stop();
                if (Piece == PieceShape.Square) ETAPA_0 = 15;
                else if  (Piece == PieceShape.Cylinder)  ETAPA_0 = 16;
                else if (Piece ==  PieceShape.Triangle) ETAPA_0 = 17;
                break;
            case 15:
                if (ETAPA_20 == 28 || ETAPA_20 == 30 || ETAPA_20 == 32 || ETAPA_20==34) ETAPA_0 = 18;
                break; 
            case 16:        
                if (ETAPA_40 == 48 || ETAPA_40 == 50 || ETAPA_40 == 52 || ETAPA_40==54) ETAPA_0 = 18;
                break;
            case 17:
                if (ETAPA_60 == 68 || ETAPA_60 == 70 || ETAPA_60 == 72 || ETAPA_60==74) ETAPA_0 = 18;
                break;
            case 18: 
                M2.MoveToLevel(0);
                if (S8) ETAPA_0 = 19;
                break;

            case 19:
                M3.MoveToPos(1);
                if (S4) ETAPA_0 = 0;
                break;                
        }
    }

void ShelfA()
    {
        switch (ETAPA_20)
        {
            case 20:
                if (ETAPA_0 == 15 && S3.isActivated) ETAPA_20 = 21;
                break;
            case 21:
                M2.MoveToLevel(1); // Movimiento vertical
                T2_A.Update(true);
                if (S9 && T2_A.done) {ETAPA_20 = 22; T2_A.Update(false);}
                break;
            case 22:
                if (C_A.pieceCount % 3 == 0) ETAPA_20 = 23;
                else if (C_A.pieceCount % 3 == 1) ETAPA_20 = 24;
                else ETAPA_20 = 25;
                break;
            case 23:
                M3.MoveToPos(2); // Suponiendo que S5 es nivel 1
                if (S5) ETAPA_20 = 26;
                break;
            case 24:
                M3.MoveToPos(3); 
                if (S6) ETAPA_20 = 26;
                break;
            case 25:
                M3.MoveToPos(4); 
                if (S7) ETAPA_20 = 26;
                break;
            case 26:
                M4.Active_and_not_forward(); // Empujar hacia la estantería
                T1.Update(true);
                 if (!S3.isActivated && T1.done) {T1.Update(false); ETAPA_20 = 27;} 
                break;
            case 27:
                M4.Stop();
                if (C_A.pieceCount % 3 != 0) ETAPA_20 = 28;
                else if (C_A.pieceCount == 3 ) ETAPA_20 = 29;
                else if (C_A.pieceCount == 6) ETAPA_20 = 31;

                else if (C_A.pieceCount == 9) ETAPA_20 = 33;
                break;
            case 28: 
                if (ETAPA_0 == 0) ETAPA_20 = 20;
            break;

             case 29:
                M9_A.Active_and_not_forward();
                if (SA_2.isActivated) ETAPA_20 = 30;
                break;
            case 30:
            M9_A.Stop();

            if (ETAPA_0 == 0) ETAPA_20 = 20;
            break;
            case 31:
                M9_A.Active_and_not_forward();
                if (SA_3.isActivated && SA_2.isActivated) ETAPA_20 = 32;
                break;
            case 32:
                M9_A.Stop();

                if (ETAPA_0==0) ETAPA_20 = 20;
                break;
            case 33: 
                if (ETAPA_80 == 88) ETAPA_20 = 34;
                break;
            case 34:
                if (ETAPA_0==0) ETAPA_20 = 20;
                break;
        }
    }
    void ShelfB()
    {
        switch (ETAPA_40)
        {
            case 40:
                if (ETAPA_0 == 16 && S3.isActivated) ETAPA_40 = 41;
                break;
            case 41:
                M2.MoveToLevel(2); // Movimiento vertical
                T2_B.Update(true);
                if (S10 && T2_B.done) {ETAPA_40 = 42; T2_B.Update(false);}
                break;
            case 42:
                if (C_B.pieceCount % 3 == 0) ETAPA_40 = 43;
                else if (C_B.pieceCount % 3 == 1) ETAPA_40 = 44;
                else ETAPA_40 = 45;
                break;
            case 43:
                M3.MoveToPos(2); // Suponiendo que S5 es nivel 1
                if (S5) ETAPA_40 = 46;
                break;
            case 44:
                M3.MoveToPos(3); 
                if (S6) ETAPA_40 = 46;
                break;
            case 45:
                M3.MoveToPos(4); 
                if (S7) ETAPA_40 = 46;
                break;
            case 46:
                M4.Active_and_not_forward(); // Empujar hacia la estantería
                T1.Update(true);
                if (!S3.isActivated && T1.done) {T1.Update(false); ETAPA_40 = 47;} 
                break;
            case 47:
                M4.Stop();
                if (C_B.pieceCount % 3 != 0) ETAPA_40 = 48;
                else if (C_B.pieceCount == 3 ) ETAPA_40 = 49;
                else if (C_B.pieceCount == 6) ETAPA_40 = 51;
                else if (C_B.pieceCount == 9) ETAPA_40 = 53;
                break;
            case 48: 
                if (ETAPA_0==0) ETAPA_40 = 40;
                break;
            case 49:
                M9_B.Active_and_not_forward();
                if (SB_2.isActivated) ETAPA_40 = 50;
                break;
            case 50:
                M9_B.Stop();
                if (ETAPA_0==0) ETAPA_40 = 40;
                break;
            case 51:
                M9_B.Active_and_not_forward();
                if (SB_2.isActivated &&SB_3.isActivated) ETAPA_40 = 50;
                break;
            case 52:
                M9_B.Stop();
                if (ETAPA_0==0) ETAPA_40 = 40;
                break;
            case 53: // Espera a Transelevador 2
                if (ETAPA_80 == 88) ETAPA_40 = 54;
                break;
            case 54:
                if (ETAPA_0==0) ETAPA_40 = 40;
                break;
        }
    }



    void ShelfC()
    {
     switch (ETAPA_60)
     {
        case 60:
            // Sincronización con Clasificación (Etapa 16 es para Triángulos/C)
            if (ETAPA_0 == 17 && S3.isActivated) ETAPA_60 = 61;
            break;
        case 61:
            M2.MoveToLevel(3); // Movimiento horizontal
            T2_C.Update(true);
            if (S11 && T2_C.done) {ETAPA_60 = 62; T2_C.Update(false);}
            break;
        case 62:
            if (C_C.pieceCount % 3 == 0) ETAPA_60 = 63;
            else if (C_C.pieceCount % 3 == 1) ETAPA_60 = 64;
            else ETAPA_60 = 65;
            break;
        case 63:
            M3.MoveToPos(2); // Columna 1
            if (S5) ETAPA_60 = 66;
            break;
        case 64:
            M3.MoveToPos(3); // Columna 2
            if (S6) ETAPA_60 = 66;
            break;
        case 65:
            M3.MoveToPos(4); // Columna 3
            if (S7) ETAPA_60 = 66;
            break;
        case 66:
            M4.Active_and_not_forward(); // Empujar pieza fuera de la plataforma T1
            T1.Update(true);
            if (!S3.isActivated && T1.done) {T1.Update(false); ETAPA_60 = 67;} 
            break;
        case 67:
            M4.Stop();
            if (C_C.pieceCount % 3 != 0) ETAPA_60 = 68;
            else if (C_C.pieceCount == 3 ) ETAPA_60 = 69;
            else if (C_C.pieceCount == 6) ETAPA_60 = 71;
            else if (C_C.pieceCount == 9) ETAPA_60 = 73; // Estantería llena, esperar evacuación
            break;
        case 68: // Retorno a reposo (transición simple)
            if (ETAPA_0==0) ETAPA_60 = 60;
            break;

        case 69: // Activar rodillos estantería para posicionar pieza
            M9_C.Active_and_not_forward();
            if (SC_2.isActivated) ETAPA_60 = 70;
            break;

        case 70: // Retorno a reposo tras llenar fila
            M9_C.Stop();
            if (ETAPA_0==0) ETAPA_60 = 60;
            break;
        case 71: 
            M9_C.Active_and_not_forward();
            if (SC_3.isActivated) ETAPA_60 = 72;
            break;
        case 72:
            M9_C.Stop();
            if (ETAPA_0 ==0) ETAPA_60 = 60;
            break;
        case 73: 
            if (ETAPA_80 == 88) ETAPA_60 = 74;
            break;

        case 74: // Reset tras evacuación completa
            if (ETAPA_0 == 0) ETAPA_60 = 60;
            break;
    }
}
    
    void Evacuation()
        {
            switch (ETAPA_80)
        {
            case 80:
                if (ETAPA_20 == 33) ETAPA_80 = 81;
                else if (ETAPA_20 != 33 && ETAPA_40 == 53) ETAPA_80 = 82;
                else if (ETAPA_20 != 33 && ETAPA_40 != 53 && ETAPA_60 == 73) ETAPA_80 = 83;
                break;
            case 81:
                M6.MoveToLevel(1);
                if (SV2) ETAPA_80 = 84;
                break;
            case 82:
                M6.MoveToLevel(2);
                if (SV3) ETAPA_80 = 84;
                break;
            case 83:
                M6.MoveToLevel(3);
                if (SV4) ETAPA_80 = 84;
                break;
            case 84:
                M7.Active_and_not_forward(); // Recoger de estantería
                if (Piece == PieceShape.Square) ETAPA_80=85;
                else if (Piece == PieceShape.Cylinder) ETAPA_80=86;
                else ETAPA_80=87;
                break;

            case 85: // A
            M9_A.Active_and_not_forward();
            M7.Active_and_not_forward();
            if (C_T.pieceCount == 9) ETAPA_80 = 88;
            break;

            case 86: // B
         M9_B.Active_and_not_forward();
            M7.Active_and_not_forward();
             if (C_T.pieceCount == 9) ETAPA_80 = 88;
            break;

             case 87: // C
             M9_C.Active_and_not_forward();
            M7.Active_and_not_forward();
            if (C_T.pieceCount == 9) ETAPA_80 = 88;
             break;
            case 88:
                M9_A.Stop();
                M9_B.Stop();
                M9_C.Stop();                
                M7.Stop();
                M5.MoveToPos(2);
                if (SH2) ETAPA_80 = 89;
                break;
            case 89:
                M6.MoveToLevel(0);
                if (SV1) ETAPA_80 = 90;
                break;
            case 90:
                M7.Active_and_forward();
                M8.SetState(true);
                if (S12.isActivated && !S13.isActivated) ETAPA_80 = 91;
                break;
            case 91:
                M7.Stop();
                M5.MoveToPos(1);
                if (!S12.isActivated) ETAPA_80 = 92;
                break;
            case 92:
                M8.SetState(false);
                C_T.ResetCounter();
                if (SV1 && SH1) ETAPA_80 = 80;
                break;
        }
    }
        }   
        


