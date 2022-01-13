# Haushaltsbuch
## Projektziel
Ziel des Projekts ist es, ein Haushaltsbuch als Desktop Anwendung zur einfachen Analyse der persönlichen Finanzen zu erstellen.
Dabei sind die Kernfunktionen
- der Import von CSV Dateien, die von den Banken zum Download angeboten werden.
- die Ansicht, Bearbeitung und Kategorisierung von Buchungen.
- die Analyse der Buchungen nach Kategorien, Regelmäßigkeit und Wert.
- das festlegen von Ausgabezielen je Kategorie und die Anzeige/Analyse von deren Einhaltung.

### Namensideen:
- Finance 4 Private Use
- Household Bugdet
- Cash Book
- Expanse Journal

### Weitere Funktionen werden:
- Erfassung der Zählerstände, wie Wasser oder Strom, und die Berechnung der vorraussichtlichen Kosten anhand des eingegenbenen Preises.
- Spar Ziele: werden aus dem monatlichen Überschuss gefüllt und können gewichtet werden, um so die Verteilung des Überschuss zu steuern. 
- Dokumenten Management, bei dem die Anwendung eine Ordnerstruktur in einem vorgegebenen Pfad verwaltet. Dabei können, aus den dort abgelegten Dokumenten Bündel erstellt werden, um bspw. Dokumente für die Steuererklärung zusammenzufassen. Die Anwendung würde diese dann, sobald sie benötigt werden, als Kopie in einem Ordner gebündelt im Downloadordner ablegen.
- Kreditrechner: Berechnung von Kreditraten bzw. der Restschuld und Tilgungszeit


## Aufbau
### Datenspeicher
Zur Speicherung der Daten wird eine verschlüsselte SQLite Datenbank verwendet. So sind die Daten grundsätzlich lokal gespeichert und es ist keine Internetverbindung notwendig. Außerdem liegen die Daten damit vollständig in der Verantwortung des Anwenders.
Zukünftig könnte eine Schnittstelle zu OneDrive oder Google Drive sinnvoll sein, um die Daten zu sichern und die Möglichkeit zu bieten, von mehreren PCs darauf zuzugreifen.
### User Interface
Für das UI wird WPF genutzt. Dadurch ist die Anwendung grundsätzlich auf Bildschirmen unterschiedlicher DPI korrekt skaliert. Zur Reduzierung der Komplexität verzichte ich auf die Nutzung von MVVM, grundsätzlich wird allerdings jede Logik die nicht zwangsläufig im UI programmiert werden muss in einer gesonderten Library programmiert.
### Library
Hier wird die gesamte Logik der Anwendung entwickelt. Außerdem werden alle Nugets, sofern nicht anders möglich, in dieser Library installiert und genutzt.


## Beispiele
![Startseite](https://github.com/SchuetteS/Haushaltsbuch/tree/main/Images/Startseite.png)

![Buchungen](https://github.com/SchuetteS/Haushaltsbuch/tree/main/Images/Buchungen.png)

## License
[Apache-2.0 License](http://www.apache.org/licenses/)
