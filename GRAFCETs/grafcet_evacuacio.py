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

        # EVACUACION
        'E80': '', 'E81': '', 'E82': '', 'E83': '', 'E84': '', 'E85': '', 'E86': '', 'E87': '',
        'E88': '', 'E89': '', 'E90': '', 'E91': '', 'E92': ''
    }
    connexions = [


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
    g = crear_grafcet_simple("G80")
    g.render(format='pdf', cleanup=True)
    g.view()
