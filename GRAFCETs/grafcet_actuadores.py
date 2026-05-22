from graphviz import Digraph
import itertools


def crear_grafcet_simple(file_name="Industrial_Plant"):

    grafcet = Digraph('GRAFCET_Simple', filename=file_name)
    grafcet.attr(fontname="monospace") 
    grafcet.attr('node', fontname="monospace")
    grafcet.attr(rankdir='TB')
    grafcet.attr(splines='ortho')
    grafcet.attr(nodesep="0.3", ranksep="0.1")
    grafcet.attr(bgcolor='transparent')
    grafcet.attr(pack='true')
    grafcet.attr(packmode='array_ti')

    # Configuració
    grafcet.attr('node', fontsize='10')
    grafcet.attr('edge', arrowsize='0.8')
    
   # Etapes i les seves accions
    etapas_acciones = {
        "E100": "M1 = OFF",
        "E101": "M1 = ON",
        "E120": "M7 = STOP",
        "E121": "M4 = ACTIVE (FORWARD)",
        "E122": "M4 = ACTIVE (\\not(FORWARD))",
        "E140": "", "E141": "M2 - Level 0", "E142": "", "E143": "M2 - Level 1",
        "E144": "", "E145": "M2 - Level 2", "E146": "", "E147": "M2 - Level 3", "E148": "",
        "E160": "", "E162": "", "E161": "M3 - Pos 1", "E163": "M3 - Pos 2",
        "E164": "", "E165": "M3 - Pos 3", "E166": "", "E167": "M3 - Pos 4", "E168": "",
        "E180": "M9_A = STOP", "E181": "M9_A = ACTIVE (\\not(FORWARD))",
        "E200": "M9_B = STOP", "E201": "M9_B = ACTIVE (\\not(FORWARD))",
        "E220": "M9_C = STOP", "E221": "M9_C = ACTIVE (\\not(FORWARD))",
        "E260": "", "E261": "M5 - Pos 1", "E262": "M5 - Pos 2", "E263": "",
        "E280": "", "E281": "M6 - Level 0", "E282": "", "E283": "M6 - Level 1",
        "E284": "", "E285": "M6 - Level 2", "E286": "", "E287": "M6 - Level 3", "E288": "",
        "E240": "M7 = STOP", "E241": "M7 = ACTIVE (\\not(FORWARD))", "E242": "M7 = ACTIVE (FORWARD)",
        "E300": "M8 = OFF", "E301": "M8 = ON",
    }

    connexions = [
   
        (("inici","E101"), '', 'E100'),
        ("E100", "x10+x12 + (COND_MANUAL*CMD_M1==1)", "E101"),
        ("E101", "x11+x13 + (COND_MANUAL*CMD_M1==0)", ('final','E100')),

        (("inici","E121,E122"), '', 'E120'),
        ("E120", "x12+x13 + (COND_MANUAL*CMD_M4==-1)", "E121"),
        ("E121", "x14 + (COND_MANUAL*CMD_M4==0)", ('final','E120')),
        ("E120", "x26+x46+x66 + (COND_MANUAL*CMD_M4==1)", "E122"),
        ("E122", "x27+x47+x67 + (COND_MANUAL*CMD_M4==0)", ('final','E120')),

        (("inici","E142,E144,E146,E148"), '', 'E140'),
        ("E140", "x18 + (COND_MANUAL*CMD_M2==0*\\not(M3.IsMoving))", "E141"),
        ("E140", "x21 + (COND_MANUAL*CMD_M2==1*\\not(M3.IsMoving))", "E143"),
        ("E140", "x41 + (COND_MANUAL*CMD_M2==2*\\not(M3.IsMoving))", "E145"),
        ("E140", "x61 + (COND_MANUAL*CMD_M2==3*\\not(M3.IsMoving))", "E147"),
        ("E141", "S8", "E142"),
        ("E142", "x19 + (COND_MANUAL*CMD_M2!=0)", ('final','E140')),
        ("E143", "S9", "E144"),
        ("E144", "x22 + (COND_MANUAL*CMD_M2!=1)", ('final','E140')),
        ("E145", "S10", "E146"),
        ("E146", "x42 + (COND_MANUAL*CMD_M2!=2)", ('final','E140')),
        ("E147", "S11", "E148"),
        ("E148", "x62 + (COND_MANUAL*CMD_M2!=3)", ('final','E140')),

        (("inici","E162,E164,E166,E168"), '', 'E160'),
        ("E160", "x19 + (COND_MANUAL*CMD_M3==0*\\not(M2.IsMoving))", "E161"),
        ("E160", "x23 + (COND_MANUAL*CMD_M3==1*\\not(M2.IsMoving))", "E163"),
        ("E160", "x24 + (COND_MANUAL*CMD_M3==2*\\not(M2.IsMoving))", "E165"),
        ("E160", "x25 + (COND_MANUAL*CMD_M3==3*\\not(M2.IsMoving))", "E167"),
        ("E161", "S4", "E162"),
        ("E162", "x0 + (COND_MANUAL*CMD_M3!=0)", ('final','E160')),
        ("E163", "S5", "E164"),
        ("E164", "x26+x46+x66) + (COND_MANUAL*CMD_M3!=1)", ('final','E160')),
        ("E165", "S6", "E166"),
        ("E166", "x26+x46+x66 + (COND_MANUAL*CMD_M3!=2)", ('final','E160')),
        ("E167", "S7", "E168"),
        ("E168", "x26+x46+x66 + (COND_MANUAL*CMD_M3!=3)", ('final','E160')),

        (("inici","E181"), '', 'E180'),
        ("E180", "x29+x31+x85 + (COND_MANUAL*CMD_M9_A==1)", "E181"),
        ("E181", "x30+x32+x88 + (COND_MANUAL*CMD_M9_A==0)", ('final','E180')),
        
        (("inici","E201"), '', 'E200'),
        ("E200", "x49+x51+x86 + (COND_MANUAL*CMD_M9_B==1)", "E201"),
        ("E201", "x50+x52+x88 + (COND_MANUAL*CMD_M9_B==0)", ('final','E200')),

        (("inici","E221"), '', 'E220'),
        ("E220", "x69+x71+x87 + (COND_MANUAL*CMD_M9_C==1)", "E221"),
        ("E221", "x70+x72+x88 + (COND_MANUAL*CMD_M9_C==0)", ('final','E220')),

        (("inici","E261,E263"), '', 'E260'),
        ("E260", "x91 + (COND_MANUAL*CMD_M5!=0*\\not(M6.IsMoving))", "E261"),
        ("E260", "x88 + (COND_MANUAL*CMD_M5==1*\\not(M6.IsMoving))", "E262"),
        ("E261", "SH1", ('final','E260')),
        ("E262", "SH2", "E263"),
        ("E263", "x89 + (COND_MANUAL*CMD_M5!=1)", ('final','E260')),

        (("inici","E282,E284,E286,E288"), '', 'E280'),
        ("E280", "x89 + (COND_MANUAL*CMD_M6==0*\\not(M5.IsMoving))", "E281"),
        ("E280", "x81 + (COND_MANUAL*CMD_M6==1*\\not(M5.IsMoving))", "E283"),
        ("E280", "x82 + (COND_MANUAL*CMD_M6==2*\\not(M5.IsMoving))", "E285"),
        ("E280", "x83 + (COND_MANUAL*CMD_M6==3*\\not(M5.IsMoving))", "E287"),
        ("E281", "SV1", "E282"),
        ("E282", "x90 + (COND_MANUAL*CMD_M6!=0)", ('final','E280')),
        ("E283", "SV2", "E284"),
        ("E284", "x84 + (COND_MANUAL*CMD_M6!=1)", ('final','E280')),
        ("E285", "SV3", "E286"),
        ("E286", "x84 + (COND_MANUAL*CMD_M6!=2)", ('final','E280')),
        ("E287", "SV4", "E288"),
        ("E288", "x84 + (COND_MANUAL*CMD_M6!=3)", ('final','E280')),

        (("inici","E241,E242"), '', 'E240'),
        ("E240", "x84+x85+x86+x87 + (COND_MANUAL*CMD_M7==1)", "E241"),
        ("E240", "x90 + (COND_MANUAL*CMD_M7==-1)", "E242"),
        ("E241", "x88 + (COND_MANUAL*CMD_M7==0)", ('final','E240')),
        ("E242", "x91 + (COND_MANUAL*CMD_M7==0)", ('final','E240')),

        (("inici","E301"), '', 'E300'),
        ("E300", "x90 + (COND_MANUAL*CMD_M8==1)", "E301"),
        ("E301", "x92 + (COND_MANUAL*CMD_M8==0)", ('final','E300')),
    ]

    # Crear etapes i accions
    for etapa, accio in etapas_acciones.items():
        # Node de l'etapa
        num=etapa.replace("E", "")
        if etapa=="E100" or etapa=="E120" or etapa=="E140"or etapa=="E160" or etapa=="E180" or etapa=="E200" or etapa=="E220" or etapa=="E240" or etapa=="E260" or etapa=="E280" or etapa=="E300":
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
            
            grafcet.node(trans_mid_node,"",shape='line', width='0.5',height='0')

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
            
            grafcet.node(trans_mid_node,"",shape='line',minlen="2", width='0.5',height='0')

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
    
    




    text_controls = """
    <B>CONDITIONS:</B>

    COND_MANUAL = x503 + x504
    M2.IsMoving = !x141 + !x143 + !145 + !147
    M3.IsMoving = !x161 + !x163 + !165 + !167
    M5.IsMoving = !x261 + !x263 
    M6.IsMoving = !x281 + !x283 + !285 + !287

    """

    html_controls = htmlitza_taula(text_controls)

    with grafcet.subgraph(name='cluster_legend') as legend:
        legend.attr(rank='sink')
        legend.attr(label="Llegenda", fontsize='10', fontname='monospace')
        legend.attr(style='rounded,dashed', color='gray')


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
    g = crear_grafcet_simple("Industrial_Plant-Actuators")
    g.render(format='pdf', cleanup=True)
    g.view()
