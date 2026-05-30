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

    }
    connexions = [
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
    S3  - Presència en plataforma transelevador 1 

    S4  - Posició horitzontal cinta principal 
    S5  - Posició horitzontal estanteria columna 1 
    S6  - Posició horitzontal estanteria columna 2 
    S7  - Posició horitzontal estanteria columna 3 

    S8  - Nivell vertical cinta 
    S9  - Nivell vertical estanteria A 
    S10 - Nivell vertical estanteria B 
    S11 - Nivell vertical estanteria C 

    S12 - Presència en cinta evacuació

    S13 - Prescència en plataforma transelevador 2 al principi
    S14 - Prescència en plataforma transelevador 2 al final

    SA_1..3 - Sensors estanteria A
    SB_1..3 - Sensors estanteria B
    SC_1..3 - Sensors estanteria C

    SH1 - Transelevador 2 posició estanteries
    SH2 - Transelevador 2 posició evacuació

    SV1 - Nivell evacuació 
    SV2 - Nivell estanteria A 
    SV3 - Nivell estanteria B 
    SV4 - Nivell estanteria C 
    """

    text_act = """
        <B>Comptadors:</B>
    C_A - Peces estanteria A
    C_B - Peces estanteria B
    C_C - Peces estanteria C

    C_EVAC_A - Evacuacions A
    C_EVAC_B - Evacuacions B
    C_EVAC_C - Evacuacions C

    

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



    text_controls = """
        <B>Timers:</B>
    T0 - Temps per deixar en la plataforma del transelevador 1
    T1 - Temps per assegurar-te que la plataforma ha deixat la peça en l'estanteria
    T2_A -  Temps control transelevador 1 vertical i horitzonal quan peça A
    T2_C -  Temps control transelevador 1 vertical i horitzonal quan peça B
    T2_B -  Temps control transelevador 1 vertical i horitzonal quan peça C

    

    <B>Condicions i Botons:</B>
    COND_AUTO = (x501+x502) * x600 * not(B_Pausa)
    COND_ACTUATORS_INITIAL = x100 * x120 * x140 * x160 * x180 * x200 * x220 * x240 * x260 * x280 * x300
    COND_MANUAL = x503 + x504
    M2_IsMoving = x141 + x143 + 145 + 147
    M3_IsMoving = x161 + x163 + 165 + 167
    M5_IsMoving = x261 + x263 
    M6_IsMoving = x281 + x283 + 285 + 287
    B_Start     - Botó d'arrencada del sistema
    B_Stop      - Aturada controlada del sistema
    B_Pausa     - Pausa del procés 
    B_Emergency - Aturada d'emergència 
    B_Reset     - Botó per sortir del mode d'emergència, útil per avaria. 



    """
    html_sensors = htmlitza_taula(text_sensors)
    html_act = htmlitza_taula(text_act)
    html_controls = htmlitza_taula(text_controls)

    with grafcet.subgraph(name='cluster_legend') as legend:
        legend.attr(rank='sink')
        legend.attr(label="Llegenda", fontsize='10', fontname='monospace')
        legend.attr(style='rounded,dashed', color='gray')

        legend.node('LEG_SENS', label=html_sensors, shape='none')
        legend.node('LEG_ACT', label=html_act, shape='none')

        legend.node('LEG_CTL', label=html_controls, shape='none')

    return grafcet
    
def htmlitza_taula(text):


    linies = text.strip().splitlines()

    rows = []

    for l in linies:

        l = l.strip()

        # línea vacía = espacio vertical
        if l == "":
            rows.append(
                "<TR><TD HEIGHT='15'></TD></TR>"
            )
        else:
            rows.append(
                f"<TR><TD ALIGN='LEFT'>{l}</TD></TR>"
            )
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
    g = crear_grafcet_simple("Industrial_Plant-Legend")
    g.render(format='pdf', cleanup=True)
    g.view()
