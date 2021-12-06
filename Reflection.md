# Left-To-Do | Reflektion

**Kim Kristiansson**
**2021-12-03**

## Kodens funktioner och struktur

### Förlängningar

(Se filen _ConsoleStringExtension.cs_)
I mitt program så har jag använt mig mycket av förlängningar på flera typer, bland annat strängar och listor. Motiveringen till det var att på ett enkelt sätt kunna skriva ut information i konsolfönstret utan att nödvändigtvis göra klassen `Task` beroende av den nativa klassen `Console`. Förlängningarna till listorna och objekten av någon _typ_ från `Task` innehåller dock flera funktionaliteter som skulle kunna tillhöra de enskilda klasserna för att behålla inkapsling av information och förenkla vidareutveckling. Dock är jag väldigt nöjd med hur arbetet med `Console` metoderna underlättades med detta sätt.

### App

Den statiska klassen _App_ innehåller de väsentliga delarna av programmet. Där interagerar användaren med programmet och presentation av resultaten organiseras där. Jag bollade mycket med mig själv om jag skulle ha en klass för inputs-hantering och en klass för visuell orientering. Klassen _App_ blev en hybrid av dem båda då jag inte kunde bestämma mig för en tydlig gräns mellan dem båda. Jag kan inte säga om jag är nöjd med resultatet eller inte, den fick bli vad den blev. Resultatet av den är i sin tur ett resultat över den grundläggande strukturen jag gjorde med de andra klasserna. Och det blev ganska likt hur jag tänkte mig att klassen skulle se ut i slutändan.

### Task

Som tidigare nämnt så ville jag separera alla klasser inom klasshierarkin `Task` från konsol och representation. Jag tyckte att det var viktigt att hålla så tydlig separation som möjligt och låta utomstående funktioner hantera dess information. Sent i utvecklingen så tillkom dock _metoder_ i klasserna så som `SetArchived()` och `SetDone()`. Dessa metoder var ursprungligen egenskaper som jag slopade för att ta anamma inkapslingen av klassernas medlemmar. Jag hade alltså inte tydlig bild nog av inkapsling innan jag startade projektet.

#### Deadline

Jag vet att det stod 2 - 3 paragrafer, men de denna som en fotnot av paragrafen ovan. Deadline blev en egen klass för sig för att förenkla utvecklingen ifall en annan typ av uppgift skulle utökas med samma funktionalitet. I _Deadline_ lagras dock endast slutdatumet, men inte antal dagar dit. Anledningen till detta var att jag ansåg att det är en fördela att endast lagra _relativa_ konstanten, som är slutdatumet, och behålla dagens datum som är ständigt i förändring.

## Använda principerna

### Arv

Klassen `Task` är superklassen till alla dem övriga klasserna och är också en abstrakt klass som endas sätter grunden för hur dess subklasser ska se ut. Den inne håller de mest nödvändigaste funktionerna och egenskaperna för att kunna hantera objekten på ett smidigt sätt. Jag ville använda mig av en abstrakt superklass för att kunna skapa flexibla subklasser som tog tillvara på dess unika egenskaper och generalisera objekten ifrån subklasserna. Klassen `Task` och `TimelessTask` är närmast identiska, men jag ville behålla det så då jag under utvecklingen inte ville förväxla faktiska objekt med generaliseringen av klasserna.

### Inkapsling

Under utvecklingens gång så tänkte jag noga på inkapsling, och i min värld så var det att ändra på medlemmarna i klasserna under kontrollerad form. Därför valde jag att använda mig av _egenskaper_ och endast tillgängliggöra `get`-metoden därifrån för att undvika att medlemmar, så som `List<T>` ändrades helt och hållet. Min bild av inkapsling var nog sann, men inte riktigt hela vägen. Det finns utrymme för att lägga detaljerna från objekten i ytterligare lager av abstraktion, så som ett lämpligt sätt att presentera enskilda objekt istället för att det skulle hanteras i en funktion. Men det gick emot min inställning inför uppgiften att så tydligt som möjligt separera information objekten och den visuella presentationen av den, så jag beslutade mig för att behålla mina egenskaper och överleva på syntetiskt socker.

### Polymorfism

Polymorfism är känner jag är en kompis som jag aldrig fått veta namnet på, trevligt att råkas. Jag har använt mig av principen under flera tillfällen i utvecklingen. Främst i mina förlängningar. Där ville jag försäkra mig själv om att de funktioner jag använder mig av fungerar och gör det jag förväntar mig; att skriva ut i konsolen och inget mer. En del av dessa förlängningar blev överlagrade med lite `bool` och `int` som parametrar för att göra en acceptabel representation av uppgifterna som användaren angett.  
De polymorfiska principerna att finna i klasserna `Task` är att de behandlas på linkande sätt, med vissa undantag så som deadline och en checklista, tack vare att de är nedärvda från samma superklass. Men vid min utvärdering nu ser jag att principen lyser lite med sin frånvaro där.

## Tester för klasser

### `TestIfPossibleToCreateDeadlineTask()`

```
[Fact]
public  void  TestDateHowManyDaysRemaining()
{
	// Arrange
	// Skiljer 7 dagar mellan dagarna
	DateTime firstDate = DateTime.ParseExact("09/12/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture);
	DateTime secondDate = DateTime.ParseExact("16/12/2021", "dd/MM/yyyy", CultureInfo.InvariantCulture);
	int expectedDays =  7; // 7 dagar

	// Act
	TimeSpan daysLeft = secondDate - firstDate;

	// Assert
	Assert.Equal(expectedDays, (int)Math.Ceiling(daysLeft.TotalDays));
}
```

Testet visar att en korrekt formaterad sträng är möjlig att konvertera till typen `DateTime`. För att kunna jämföra två datum krävs _structen_ `TimeSpan` där tiden mellan två datum kan kalkyleras. Det är sedan möjligt att få ut den beräknade tiden i minuter, timmar, dagar, mm. I detta fallet så gällde det dagar.
`TotalDays` är en egenskap hos `TimeSpan` som återger en `double` på antal dagar som skiljer mellan de två datumen.

Detta testet var viktigt för att testa dessa funktioner som jag tidigare var obekant med. Det var dessa metoder jag senare använde för att hantera värdena från användaren korrekt och ledde till att jag kunde uppfylla kraven som ställdes på `DeadlineTask`. Dock så använde jag mig av `DateTime.TryParseExact()` i mitt program som fungerar ungefär likadant.

### `TestIfSetToDoneIsPossible()`

```
[Fact]

public  void  TestIfSetToDoneIsPossible()
{
	// Arrange
	ToDo todo = new ToDo();
	List<Task> taskList = new  List<Task>();
	todo.TaskList.Add(new TimelessTask("OK"));
	todo.TaskList.Add(new DeadlineTask("OK", new Deadline(new DateTime(2021, 12, 9))));
	todo.TaskList.Add(new ChecklistTask("OK9", new List<TimelessTask>() {new TimelessTask("ok0"), new TimelessTask("ok1"), new TimelessTask("ok2") }));

	// Act

	todo.TaskList[0].SetDone(true);
	todo.TaskList[1].SetDone(true);
	ChecklistTask checklistTask = (ChecklistTask)todo.TaskList[2];
	checklistTask.TimelessTaskList[0].SetDone(true);
	checklistTask.TimelessTaskList[1].SetDone(true);
	checklistTask.TimelessTaskList[2].SetDone(true);
	checklistTask.CheckIfAllTasksAreDone();

	// Assert
	Assert.True(todo.TaskList[0].IsDone);
	Assert.True(todo.TaskList[1].IsDone);
	Assert.True(todo.TaskList[2].IsDone);
}
```

Detta test vill jag egentligen bara ta upp för att för att upplysa om mina tankar kring att arbeta med `ChecklistTask`. Klassens konstruktor tar emot en lista men `TimelessTask` som kan bockas som _klara_ eller _arkiverade_ med hjälp av ärvda metoder. När detta sker så såg jag till att metoden `CheckIfAllTasksAreDone()` exekverades. Denna metod såg itererade igenom listan med `TimelessTask` och avbröt iterationen ifall alla uppgifter inte var klara. Om alla uppgifter i listan var markerade som klara så markerades även _checklistan_ som färdig. I testet testas dock alla typer av uppgifter, men det jag var ute efter var `ChecklistTask`.
Test är ett _proof of concept_ på att min jag tänkte i rätt bana.
