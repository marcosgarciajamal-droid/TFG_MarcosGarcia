Contingut del repositori
========================
LATEX: Conté tots els fitxers LaTeX de la memòria final, incloent-hi les carpetes IMAGES i PDF_GRAFCET.

SCRIPT_USED:Scripts en C# utilitzats a Unity: scripts generals, sensors, MQTT i ASRS.

GRAFCETS:Scripts Python per generar GRAFCETs amb Graphviz. Inclou un venv preparat per Linux.

IMAGES_RAPIDSCADA:Imatges utilitzades dins de Rapid SCADA generades amb IA.

Notes
=====
Per executar els scripts Python de GRAFCETS, cal activar el venv i tenir Graphviz instal·lat.

Execució del Sistema
====================

Per executar correctament el sistema és necessari tenir instal·lats Rapid SCADA i Mosquitto al mateix dispositiu (localhost).

	* El projecte SCADA es troba a PROJECTE_FINAL/SCADA i s’ha d’importar des de l’aplicació Administrator de Rapid SCADA i publicar-se a la Webstation.
	* El projecte Unity es troba a PROJECTE_FINAL/UNITY i es pot obrir amb Unity Hub per modificar-lo.
	* També es proporciona una versió executable a PROJECTE_FINAL/UNITY/BuildApp per executar el sistema directament sense necessitat de modificar-lo.

La comunicació entre Unity i Rapid SCADA es realitza mitjançant MQTT en entorn local.