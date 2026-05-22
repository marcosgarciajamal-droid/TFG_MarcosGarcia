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
        # --- DUAL TWEEN ---
        'EDT0': '', 'EDT10': '', 'EDT11': 'T=10s', 'EDT12': '', 'EDT13': '', 'EDT14': '', 'EDT15': '', 
        'EDT16': '', 'EDT17': '','EDT18': '', 'EDT19': '','EDT21': '',

    }
    connexions = [

        # --- GRAFCET MODO (500) ---
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

            
    ]

    # Crear etapes i accions
    for etapa, accio in etapas_acciones.items():
        # Node de l'etapa
        num=etapa.replace("E", "")
        if etapa=="E0" or etapa=="E20" or etapa=="E40"or etapa=="E60" or etapa=="E80" or etapa =="E500" or etapa=="E600" or etapa=="EDT0":
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
    g = crear_grafcet_simple("GDT0")
    g.render(format='pdf', cleanup=True)
    g.view()
