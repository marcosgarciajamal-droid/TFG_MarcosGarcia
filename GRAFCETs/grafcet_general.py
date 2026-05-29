from graphviz import Digraph
import itertools


def crear_grafcet_simple(file):

    grafcet = Digraph('GRAFCET_Simple', filename=file)
    grafcet.attr(fontname="monospace") 
    grafcet.attr('node', fontname="monospace")
    grafcet.attr(rankdir='TB')
    grafcet.attr(splines='ortho')
    grafcet.attr(nodesep="0.7", ranksep="0.1")
    grafcet.attr(bgcolor='transparent')
    grafcet.attr(pack='true')
    grafcet.attr(packmode='array_ti')


    # Configuració
    grafcet.attr('node', fontsize='10')
    grafcet.attr('edge', arrowsize='0.8')
    

    # Etapes i les seves accions
    etapas_acciones = {
        'E500': 'LED AUTO OFF | LED MANUAL OFF', 'E501': 'LED AUTO ON', 'E502': '', 'E503': 'LED MANUAL ON', 'E504': '',

        'E600': 'LED_EMERGENCY OFF', 
        'E601': 'F/G0\{0\} F/G20\{20\} F/G40\{40\} \\n F/G60\{60\} F/80\{80\} F/GDT0\{0\} F/G500\{503\} | LED_EMERGENCY ON',
        'E602': '',
# CLASIFICACION (0)
        'E0': '', 'E1': '', 'E10':' P | SpawnPiece', 'E11':'', 'E12':'', 'E13': 'T=0.15s', 'E14': '', 'E15': '', 'E16': '', 'E17': '', 'E18': '', 'E19': '',

        # --- DUAL TWEEN ---
        'EDT0': '', 'EDT10': '', 'EDT11': 'T=10s', 'EDT12': '', 'EDT13': '', 'EDT14': '', 'EDT15': '', 
        'EDT16': '', 'EDT17': '','EDT18': '', 'EDT19': '','EDT21': '',

        # ESTANTERIA A
        'E20': '', 'E21': 'T=1s', 'E22': '', 'E23': '', 'E24': '', 'E25': '', 'E26': 'T=2s',
        'E27': 'P | C_A=C_A+1', 'E28': '', 'E29': '', 'E30': '', 'E31': '', 'E32': '', 'E33': '', 'E34': 'P | C_A=0 | CEvacA++',
        
        # ESTANTERIA B
        'E40': '', 'E41': 'T=2s', 'E42': '', 'E43': '', 'E44': '', 'E45': '', 'E46': 'T=2s',
        'E47': ' P | C_B=C_B+1', 'E48': '', 'E49': '', 'E50': '', 'E51': '', 'E52': '', 'E53': '', 'E54': 'P | C_B=0 | CEvacB++',
        
        # ESTANTERIA C
        'E60': '', 'E61': 'T=3s', 'E62': '', 'E63': '', 'E64': '', 'E65': '', 'E66': 'T=2s',
        'E67': 'P | C_C=C_C+1', 'E68': '', 'E69': '', 'E70': '', 'E71': '', 'E72': '', 'E73': '', 'E74': 'P | C_C=0 | CEvacC++',
        
        # EVACUACION
        'E80': '', 'E81': '', 'E82': '', 'E83': '', 'E84': '', 'E85': '', 'E86': '', 'E87': '',
        'E88': '', 'E89': '', 'E90': '', 'E91': '', 'E92': ''
    }
    connexions = [

        # --- GRAFCET MODO (500) ---
        (("inici","E502, E504"), '', 'E500'),
        ('E500', 'Selector == 0 * B_Start * x600', 'E501'),
        ('E500', 'Selector == 1 * B_Start * x600', 'E503'),
        ('E501', '\\not(B_Stop)', 'E502'),
        ('E502', ' x0', ('final', 'E500')),
        ('E503', '(Selector != 1 * \\not(B_Stop)) + x602', 'E504'),
        ('E504', 'CI * COND_ACTUATORS_INITIAL', ('final', 'E500')),

        (("inici","E602"), '', 'E600'),
        ('E600', '\\not(B_Emergency) + xDT21', 'E601'),
        ('E601', 'B_Emergency * B_Reset', 'E602'),
        ('E602', 'x500', ('final', 'E600')),

        (("inici","E19"),'','E0'),
        ('E0', 'CI', 'E1'),
        ('E1', 'COND_AUTO', 'E10'),
        ('E10', 'COND_AUTO * S1', 'E11'),
        ('E11', 'COND_AUTO * S2', 'E12'),
        ('E12', 'COND_AUTO * S3', 'E13'),
        ('E13', 't/T0/0.15s', 'E14'),
        ('E14', 'COND_AUTO * Piece_A', 'E15'),
        ('E14', 'COND_AUTO * Piece_B', 'E16'),
        ('E14', 'COND_AUTO * Piece_C', 'E17'),
        ('E15', 'COND_AUTO * (x28+x30+x32+x34)', 'E18'),
        ('E16', 'COND_AUTO * (x48+x50+x52+x54)', 'E18'),
        ('E17', 'COND_AUTO * (x68+x70+x72+x74)', 'E18'),
        ('E18', 'COND_AUTO * x142', 'E19'),
        ('E19', 'COND_AUTO * x162', ('final', 'E0')),

                # --- GRAFCET DUAL TWEEN ---
        (("inici","EDT0"), '', 'EDT0'),

        ('EDT0', 'x10', 'EDT10'),
        ('EDT10', 'x11', 'EDT11'),
        ('EDT11', 'x12', 'EDT12'),
        ('EDT11', 't/TDT0/10s * \\not(x12)', 'EDT21'),
        ('EDT12', 'x13', 'EDT13'),
        ('EDT13', 'x14', 'EDT14'),
        ('EDT14', 'x15', 'EDT15'),
        ('EDT14', 'x16', 'EDT16'),
        ('EDT14', 'x17', 'EDT17'),
        ('EDT15', 'x18', 'EDT18'),
        ('EDT16', 'x18', 'EDT18'),
        ('EDT17', 'x18', 'EDT18'),
        ('EDT18', 'x19', 'EDT19'),
        ('EDT19', 'x0', ('final', 'EDT0')),
        ('EDT21', 'x600 * x0', ('final', 'EDT0')),

                #ESTANTERIA A
        (("inici","E28,E30,E32,E34"),'','E20'),
        ('E20','COND_AUTO * x15*S3' ,'E21'),
        ('E21','COND_AUTO * x144 * t/T2_A/1s' ,'E22'),
        ('E22','COND_AUTO * C_A mod 3 == 0 ' ,'E23'),
        ('E22','COND_AUTO * C_A mod 3 == 1 ' ,'E24'),
        ('E22','COND_AUTO * C_A mod 3 == 2 ' ,'E25'),
        ('E23','COND_AUTO * x164' ,'E26'),
        ('E24','COND_AUTO * x166' ,'E26'),
        ('E25','COND_AUTO * x168' ,'E26'),
        ('E26','COND_AUTO * \\not(S3) * t/T1/2s' ,'E27'),
        ('E27','COND_AUTO * C_A mod 3!= 0' ,'E28'),
        ('E28','COND_AUTO * x0' ,('final','E20')),
        ('E27','COND_AUTO * C_A==3' ,'E29'),
        ("E29",'COND_AUTO * SA_2' ,'E30'),
        ("E30",'COND_AUTO * x0' ,('final','E20')),
        ('E27','COND_AUTO * C_A==6' ,'E31'),
        ('E31','COND_AUTO * SA_2*SA_3' ,'E32'),
        ("E32",'COND_AUTO * x0' ,('final','E20')),
        ('E27','COND_AUTO * C_A==9' ,'E33'),
        ('E33','COND_AUTO * x88' ,'E34'),
        ("E34",'COND_AUTO * x0' ,('final','E20')),

        #ESTANTERIA B
        (("inici","E48,E50,E52,E54"),'','E40'),
        ('E40','COND_AUTO * x16*S3' ,'E41'),
        ('E41','COND_AUTO * x146 * t/T2_B/2s' ,'E42'),
        ('E42','COND_AUTO * C_B mod 3 == 0 ' ,'E43'),
        ('E42','COND_AUTO * C_B mod 3 == 1 ' ,'E44'),
        ('E42','COND_AUTO * C_B mod 3 == 2 ' ,'E45'),
        ('E43','COND_AUTO * x164' ,'E46'),
        ('E44','COND_AUTO * x166' ,'E46'),
        ('E45','COND_AUTO * x168' ,'E46'),
        ('E46','COND_AUTO * \\not(S3) * t/T1/2s' ,'E47'),
        ('E47','COND_AUTO * C_B mod 3 !=0' ,'E48'),
        ('E48','COND_AUTO * x0' ,('final','E40')),
        ('E47','COND_AUTO * C_B==3' ,'E49'),
        ("E49","COND_AUTO * SB_2" ,'E50'),
        ("E50", 'COND_AUTO * x0' ,('final','E40')),
        ('E47','COND_AUTO * C_B==6' ,'E51'),
        ('E51','COND_AUTO * SB_2*SB_3' ,'E52'),
        ("E52",'COND_AUTO * x0' ,('final','E40')),
        ('E47','COND_AUTO * C_B==9' ,'E53'),
        ('E53','COND_AUTO * x88' ,'E54'),
        ("E54",'COND_AUTO * x0' ,('final','E40')),

            #ESTANTERIA C
        (("inici","E68,E70,E72,E74"),'','E60'),
        ('E60','COND_AUTO * x17*S3' ,'E61'),
        ('E61','COND_AUTO * x148 * t/T2_C/2.5s' ,'E62'),
        ('E62','COND_AUTO * C_C mod 3 == 0' ,'E63'),
        ('E62','COND_AUTO * C_C mod 3 == 1 ' ,'E64'),
        ('E62','COND_AUTO * C_C mod 3 == 2' , 'E65'),
        ('E63','COND_AUTO * x162' ,'E66'),
        ('E64','COND_AUTO * x164' ,'E66'),
        ('E65','COND_AUTO * x166' ,'E66'),
        ('E66','COND_AUTO * \\not(S3) * t/T2/2s' ,'E67'),
        ('E67','COND_AUTO * C_C mod 3!=0' ,'E68'),
        ('E68','COND_AUTO * x0' ,('final','E60')),
        ('E67','COND_AUTO * C_C==3' ,'E69'),
        ("E69","COND_AUTO * SC_2" ,'E70'),
        ("E70",'COND_AUTO * x0' ,('final','E60')),
        ('E67','COND_AUTO * C_C==6' ,'E71'),
        ('E71','COND_AUTO * SC_2*SC_3' ,'E72'),
        ("E72",'COND_AUTO * x0' ,('final','E60')),
        ('E67','COND_AUTO * C_C==9' ,'E73'),
        ('E73','COND_AUTO * x88' ,'E74'),
        ("E74",'COND_AUTO * x0' ,('final','E60')),
        
        #EVACUACIO
        (("inici","E92"), '', 'E80'),
        ('E80', 'COND_AUTO * x33', 'E81'),
        ('E80', 'COND_AUTO * \\not(x33) * x53', 'E82'),
        ('E80', 'COND_AUTO * \\not(x33) * \\not(x53) * x73', 'E83'),
        ('E81', 'COND_AUTO * x284', 'E84'),
        ('E82', 'COND_AUTO * x286', 'E84'),
        ('E83', 'COND_AUTO * x288', 'E84'),
        ('E84', 'COND_AUTO * Piece_A', 'E85'),
        ('E84', 'COND_AUTO * Piece_B', 'E86'),
        ('E84', 'COND_AUTO * Piece_C', 'E87'),
        ('E85', 'COND_AUTO * S14', 'E88'),
        ('E86', 'COND_AUTO * S14', 'E88'),
        ('E87', 'COND_AUTO * S14', 'E88'),
        ('E88', 'COND_AUTO * x263', 'E89'),
        ('E89', 'COND_AUTO * x282', 'E90'),
        ('E90', 'COND_AUTO * S12 * \\not(S13)', 'E91'),
        ('E91', 'COND_AUTO * \\not(S12)', 'E92'),
        ('E92', 'COND_AUTO * x280 * x260', ('final', 'E80')),
    ]

    # Crear etapes i accions
    for etapa, accio in etapas_acciones.items():
        # Node de l'etapa
        num=etapa.replace("E", "")
        if etapa=="E0" or etapa=="E20" or etapa=="E40"or etapa=="E60" or etapa=="E80" or etapa =="E500" or etapa=="E600" or etapa=="DT0":
            grafcet.node(etapa, num, shape='square',peripheries="2", width='0.7',fixedsize='true')
        else:
            grafcet.node(etapa, num, shape='square', width='0.7',fixedsize='true')
        if accio!="":
            # Node de l'acció
            accio_node = f'{etapa}_A'
            grafcet.node(accio_node, aplicar_overline(accio), shape='record', fontsize='9')
            
            # Línia perpendicular entre etapa i acció
            with grafcet.subgraph() as s:
                s.attr(rank='same')
                s.edge(f"{etapa}", f"{accio_node}",
                       arrowhead='none', style='solid', color='black')

#            grafcet.edge(etapa, accio_node, label="AAA", decorate="true", style='solid', color='black',
#                        arrowhead='none', constraint='true')

    
    unique_id = itertools.count()
    nodes_intermediaris = {}
    nodes_intermediaris_dest = {}

    for origen, transicio,desti in connexions:
        if isinstance(desti, tuple) and desti[0] == "final":
            # Si és ("final", "E{num}")
            etapa = desti[1]              # ex: "E1"
            num = etapa.replace("E", "")  # ex: "0"
            final_node = f'F_{next(unique_id)}'       # nou node "final"
            
            # Node final amb el número pelat

            grafcet.node(final_node, num, shape='square',
                         width='0.3', fixedsize='true', color='transparent')
            
            # Fletxa cap avall des de l'origen fins al node final
            trans_mid_node=f"NM_{next(unique_id)}"
            trans_text_node=f"NMT_{next(unique_id)}"
            
            grafcet.node(trans_mid_node,"",shape='box', width='0.5',height='0.02')

            grafcet.edge(f"{trans_mid_node}:s", f"{final_node}:n", constraint='true', concentrate='true')
            grafcet.node(trans_text_node,aplicar_overline(transicio), shape='rect', color='transparent')
            with grafcet.subgraph() as s:
                s.attr(rank='same')
                s.edge(f"{trans_mid_node}:w", f"{trans_text_node}:e",
                       arrowhead='none', xlabel="",style='solid', color='transparent')

            grafcet.edge(f"{origen}:s", f"{trans_mid_node}:n",
                         decorate="true",
                         constraint='true',
                         concentrate='true',
                         fontsize='12',
                         arrowhead='none',
                         dir='forward')
        elif isinstance(origen, tuple) and origen[0] == "inici":
            # Si és ("final", "E{num}")
            etapa = origen[1]              # ex: "E1"
            num = etapa.replace("E", "")  # ex: "0"
            initial_node = f'I_{next(unique_id)}'       # nou node "final"
            
            # Node final amb el número pelat
            grafcet.node(initial_node,num, shape='square',
                         width='0.3', fixedsize='true', color='transparent')
            
            # Fletxa cap avall des de l'origen fins al node final
            
            grafcet.edge(f"{initial_node}:s", f"{desti}:n",
                         decorate="true",
                         constraint='true',
                         concentrate='false',
                         dir='forward')
        else:
            if origen not in nodes_intermediaris:
                inter_node = f"N_{next(unique_id)}"
                grafcet.node(inter_node,  shape='point', width='0', style='invis')
                grafcet.edge(f"{origen}:s", F"{inter_node}:n", minlen="1", constraint='true', concentrate='true',arrowhead='none')
                nodes_intermediaris[origen] = inter_node
            else: 
                inter_node = nodes_intermediaris[origen]
            
            trans_mid_node=f"NM_{next(unique_id)}"
            trans_text_node=f"NMT_{next(unique_id)}"
            
            grafcet.node(trans_mid_node,"",shape='box',minlen="2", width='0.5',height='0.02')

            grafcet.edge(f"{inter_node}:s", f"{trans_mid_node}:n",constraint='true',concentrate='true',arrowhead='none')
            grafcet.node(trans_text_node,aplicar_overline(transicio), shape='rect', color='transparent')
            with grafcet.subgraph() as s:
                s.attr(rank='same')
                s.edge(f"{trans_mid_node}", f"{trans_text_node}",
                       arrowhead='none', xlabel="",style='solid', color='transparent')
                
            
            if desti not in nodes_intermediaris_dest:
                inter_node_dest = f"ND_{next(unique_id)}"
                grafcet.node(inter_node_dest,  shape='point', width='0', style='invis')
                grafcet.edge(f"{inter_node_dest}:s", f"{desti}:n", minlen="1", constraint='true', concentrate='true',arrowhead='none')
                nodes_intermediaris_dest[desti] = inter_node_dest
            else: 
                inter_node_dest = nodes_intermediaris_dest[desti]
            grafcet.edge(f"{trans_mid_node}:s", f"{inter_node_dest}:n", minlen="2", decorate="true",constraint='true',concentrate='true', arrowhead='none',fontsize='12')
    
    
    text_sensors = """
    <B>Sensors:</B>
    S1  - Presència peça entrada cinta
    S2  - Sensor òptic (tipus peça A/B/C)
    S3  - Presència en plataforma transelevador 1 (T1)

    S4  - Posició horitzontal cinta principal (T1)
    S5  - Posició horitzontal estanteria columna 1 (T1)
    S6  - Posició horitzontal estanteria columna 2 (T1)
    S7  - Posició horitzontal estanteria columna 3 (T1)

    S8  - Nivell vertical cinta (T1)
    S9  - Nivell vertical estanteria A (T1)
    S10 - Nivell vertical estanteria B (T1)
    S11 - Nivell vertical estanteria C (T1)

    S12 - Presència en cinta evacuació

    S13 - Prescència en plataforma transelevador 2 al principi
    S14 - Prescència en plataforma transelevador 2 al final

    SA_1..3 - Sensors estanteria A
    SB_1..3 - Sensors estanteria B
    SC_1..3 - Sensors estanteria C

    SH1 - Transelevador 2 posició estanteries
    SH2 - Transelevador 2 posició evacuació

    SV1 - Nivell evacuació (T2)
    SV2 - Nivell estanteria A (T2)
    SV3 - Nivell estanteria B (T2)
    SV4 - Nivell estanteria C (T2)
    """

    text_act = """
    <B>Actuadors:</B>
    M1   - Cinta principal
    M2   - Moviment vertical transelevador 1
    M3   - Moviment horitzontal transelevador 1
    M4   - Rodets plataforma T1

    M5   - Moviment horitzontal transelevador 2
    M6   - Moviment vertical transelevador 2
    M7   - Rodets plataforma T2
    M8   - Cinta evacuació

    M9_A - Rodets estanteria A
    M9_B - Rodets estanteria B
    M9_C - Rodets estanteria C
    """

    text_counters = """
    <B>Comptadors:</B>
    C_A - Peces estanteria A
    C_B - Peces estanteria B
    C_C - Peces estanteria C

    C_EVAC_A - Evacuacions A
    C_EVAC_B - Evacuacions B
    C_EVAC_C - Evacuacions C
    """
    text_timers = """
    <B>Timers:</B>
    T0 - Temps per deixar en la plataforma del transelevador 1
    T1 - Temps per assegurar-te que la plataforma ha deixat la peça en l'estanteria
    T2_A -  Temps control transelevador 1 vertical i horitzonal quan peça A
    T2_C -  Temps control transelevador 1 vertical i horitzonal quan peça B
    T2_B -  Temps control transelevador 1 vertical i horitzonal quan peça C
    """

    text_controls = """
    <B>Condicions i Botons:</B>

    COND_AUTO = (x501+x502) * x600 * not(B_Pausa)
    COND_ACTUATORS_INITIAL = x100 * x120 * x140 * x160 * x180 * x200 * x220 * x240 * x260 * x280 * x300
    B_Start     - Botó d'arrencada del sistema
    B_Stop      - Aturada controlada del sistema
    B_Pausa     - Pausa del procés (atura l'avanç del GRAFCET)
    B_Emergency - Aturada d'emergència (reset general segur)
    B_Reset     - Botó per sortir del mode d'emergència, útil per avaria. 
    """
    html_sensors = htmlitza_taula(text_sensors)
    html_act = htmlitza_taula(text_act)
    html_count = htmlitza_taula(text_counters)
    html_timers = htmlitza_taula(text_timers)
    html_controls = htmlitza_taula(text_controls)

    with grafcet.subgraph(name='cluster_legend') as legend:
        legend.attr(rank='sink')
        legend.attr(label="Llegenda", fontsize='10', fontname='monospace')
        legend.attr(style='rounded,dashed', color='gray')

        legend.node('LEG_SENS', label=html_sensors, shape='none')
        legend.node('LEG_ACT', label=html_act, shape='none')
        legend.node('LEG_CNT', label=html_count, shape='none')
        legend.node('LEG_TMR', label=html_timers, shape='none')
        legend.node('LEG_CTL', label=html_controls, shape='none')

    return grafcet
    
def htmlitza_taula(taula):

    linies = [l.strip() for l in taula.strip().splitlines() if l.strip()]

    # Construir la taula HTML per justificar a l'esquerra
    taula_html = (
        "<<TABLE BORDER='0' CELLBORDER='0' CELLSPACING='0'>"
        + "".join(f"<TR><TD ALIGN='LEFT'>{l}</TD></TR>" for l in linies)
        + "</TABLE>>"
    )
    return taula_html


def aplicar_overline(texto):
    OVER = "\uFE26"  # combining overline
    
    def overline_string(s):
        # aplica \u0305 después de cada caracter
        return "".join(ch + OVER for ch in s)

    resultado = ""
    i = 0
    while i < len(texto):
        if texto.startswith(r"\not(", i):  # detecta secuencia
            i += len(r"\not(")
            # buscamos cierre ')'
            j = texto.find(")", i)
            if j == -1:
                raise ValueError("Falta cierre de ) en \\not(...)")
            contenido = texto[i:j]
            resultado += overline_string(contenido)
            i = j + 1
        else:
            resultado += texto[i]
            i += 1
    return resultado

if __name__ == "__main__":
    g = crear_grafcet_simple("Industrial_Plant-General")
    g.render(format='pdf', cleanup=True)
    g.view()
