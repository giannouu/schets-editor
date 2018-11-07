# schets-editor

De schets houdt een List van Tekeningen bij. (List<Tekening> schets.tekeningList)
De SchetsControl heeft een paar functies om die list van tekeningen te manipuleren.
Note: De tools hebben op hun beurt weer toegang tot de schetscontrol.

### Wat is een Tekening?
Een tekening is een object dat een figuur op de schets representeert.
Elke Type Tool voegt zijn Type Tekening toe aan de schets.
Een Tekening biedt de functionaliteit om zichzelf makkelijk te tekenen met de bijbehorende Graphics. (Tekening.Teken(Graphics g))
Een Tekening biedt de functionaliteit om makkelijk te kunnen checken of een punt in de Tekening ligt, bedoelt voor o.a. gummen. (Tekening.isAtPoint(Point p))

Tekening is een abstracte superklasse van alle Tekening subclasses, die wel voor specifieke tekeningen staan.


Door slechte planning is de extra functionaliteit en opslaan/laden niet geimplementeerd
