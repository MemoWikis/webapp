class SourceTexts
{
    public const string ShortSourceTextEN = @"3. Many-to-Many:
        In a many-to-many relationship, each side of the relationship can contain multiple rows.
        In this example, each book is allowed to have multiple authors. Therefore, I created a lookup table (also known as a ""junction table"") that stores both the AuthorId and the BookId.
        These two columns could be configured to be the primary key of the table (in which case they would be a ""composite primary key"" or simply ""composite key""), or you could create a separate column to be the primary key.
        Note that the Books table doesn't have AuthorId in this case. That column has been moved to the AuthorBooks table so that we can have many AuthorIds for the same BookId.";


    //https://github.com/s-shemmee/SQL-101
    public const string LongSourceTextEN = @"Welcome to the SQL 101 repository, your beginner's guide to SQL database programming. Whether you're new to databases or have some programming experience, this repository provides step-by-step guidance, code examples, exercises, and resources to help you master SQL. Let's unlock the power of data with SQL!
        
        Installation Guide
        To get started with SQL, you need to install a relational database management system (RDBMS) on your computer. Here are the installation instructions for the most popular RDBMS:
        
        MySQL
        Visit the official MySQL website at https://dev.mysql.com/downloads/installer/.
        Download the MySQL Installer for your operating system.
        Run the installer and follow the on-screen instructions.
        During the installation, select the ""MySQL Server"" component.
        Choose the appropriate setup type (e.g., Developer Default or Server Only).
        Set a root password for the MySQL Server.
        Complete the installation process.
        Verify the installation by opening a command prompt and running the following command:
        mysql --version
        You should see the installed MySQL version printed on the console.
        
        PostgreSQL
        Go to the official PostgreSQL website at https://www.postgresql.org/download/.
        Choose your operating system and download the PostgreSQL installer.
        Run the installer and follow the on-screen instructions.
        During the installation, select the components you want to install, including the PostgreSQL Server and command-line tools.
        Choose the installation directory and port number (the default values are usually fine).
        Set a password for the PostgreSQL superuser (postgres).
        Complete the installation process.
        Verify the installation by opening a command prompt and running the following command:
        psql --version
        You should see the installed PostgreSQL version printed on the console.
        
        SQLite
        Visit the official SQLite website at https://www.sqlite.org/download.html.
        Download the precompiled binaries for your operating system.
        Extract the downloaded file to a directory of your choice.
        Add the directory containing the SQLite binary to your system's PATH environment variable.
        Verify the installation by opening a command prompt and running the following command:
        sqlite3 --version
        You should see the installed SQLite version printed on the console.
        
        Choose the RDBMS that best suits your needs and follow the corresponding installation instructions. Once you have successfully installed an RDBMS, you can proceed with the SQL lessons and exercises provided in this repository.
        
        Introduction to SQL
        Structured Query Language (SQL) is a powerful and widely used language for managing and manipulating relational databases. SQL allows you to interact with databases to store, retrieve, update, and delete data. In this section, we will cover the fundamental concepts and syntax of SQL.
        
        Database Basics
        A database is an organized collection of data stored in a structured format. It consists of tables, which hold the data, and relationships between the tables. Each table consists of rows (also known as records) and columns (also known as fields). Columns define the type of data that can be stored, such as text, numbers, or dates.
        
        SQL Statements
        SQL operates through various statements that allow you to perform different actions on the database. The most common SQL statements are:
        
        SELECT: Retrieves data from one or more tables.
        INSERT: Adds new data into a table.
        UPDATE: Modifies existing data in a table.
        DELETE: Removes data from a table.
        Syntax and Structure
        SQL statements follow a specific syntax and structure. Here's a basic structure of a SELECT statement:
        
        SELECT column1, column2, ...
        FROM table_name
        WHERE condition;
        SELECT specifies the columns you want to retrieve from the table.
        FROM specifies the table you want to retrieve data from.
        WHERE (optional) specifies the conditions for filtering the data.
        Querying Data
        To retrieve data from a database, you use the SELECT statement. You can specify the columns you want to retrieve and apply various conditions to filter the data. Here's an example of a simple SELECT statement:
        
        SELECT column1, column2
        FROM table_name;
        This statement retrieves the values from column1 and column2 in the table_name table.
        
        Filtering Data
        You can filter the retrieved data using the WHERE clause. It allows you to specify conditions to match specific records. For example:
        
        SELECT column1, column2
        FROM table_name
        WHERE condition;
        The condition can be a comparison between columns or values using operators like =, <>, <, >, <=, >=. You can also use logical operators like AND, OR, NOT to combine multiple conditions.
        
        Sorting Data
        You can sort the retrieved data using the ORDER BY clause. It allows you to specify the columns to sort the data by. For example:
        
        SELECT column1, column2
        FROM table_name
        ORDER BY column1 ASC, column2 DESC;
        This statement sorts the data in ascending order based on column1 and descending order based on column2.
        
        Modifying Data
        In addition to retrieving data, SQL allows you to modify the data stored in a database. This section covers the basic SQL statements for inserting, updating, and deleting data, and explains their impact on the database.
        
        Inserting Data
        To add new data into a table, you use the INSERT statement. Here's an example:
        
        INSERT INTO table_name (column1, column2, ...)
        VALUES (value1, value2, ...);
        This statement inserts a new row into table_name with the specified values for column1, column2, and so on.
        
        Updating Data
        The UPDATE statement is used to modify existing data in a table. Here's an example:
        
        UPDATE table_name
        SET column1 = value1, column2 = value2, ...
        WHERE condition;
        This statement updates the values of column1, column2, and so on in table_name that match the specified condition.
        
        Deleting Data
        To remove data from a table, you use the DELETE statement. Here's an example:
        
        DELETE FROM table_name
        WHERE condition;
        This statement deletes the rows from table_name that match the specified condition.
        
        Note: Modifying data in a database should be done with caution, as it can permanently alter or remove data. Always double-check your statements and ensure they are targeting the correct data before executing them.
        
        Data Types and Constraints
        SQL supports various data types to store different kinds of data in tables. Additionally, you can apply constraints to enforce rules and maintain data integrity. Here are some commonly used data types and constraints:
        
        Data Types
        INTEGER: Represents whole numbers.
        FLOAT: Represents floating-point numbers.
        VARCHAR: Represents variable-length character strings.
        DATE: Represents a date without a time component.
        BOOLEAN: Represents true or false values.
        These are just a few examples, and different database systems may support additional data types.
        
        Constraints
        Primary Key: Ensures the uniqueness of a column's value in a table, typically used to uniquely identify each row.
        Foreign Key: Establishes a relationship between two tables, enforcing referential integrity.
        Unique Constraint: Ensures the uniqueness of values in one or more columns.
        Check Constraint: Defines a condition that must be true for a row to be valid.
        These constraints help maintain data integrity, enforce data relationships, and prevent invalid data from being inserted or modified.
        
        Understanding data types and constraints is crucial for designing and creating well-structured databases that accurately represent the real-world entities and relationships.
        
        This section has covered the basics of modifying data in a database using SQL statements. It has also introduced data types and constraints that help define the structure and integrity of the data.
        
        As you progress, you'll explore more advanced techniques and features of SQL, including working with multiple tables, aggregating data, and optimizing query performance.
        
        Joins and Relationships
        In a relational database, data is often spread across multiple tables, and relationships are established between them. Understanding relationships and using JOIN statements allows you to retrieve related data from multiple tables efficiently.
        
        Relationships in Databases
        There are three common types of relationships in databases:
        
        One-to-One: A relationship where each record in one table is associated with at most one record in another table.
        One-to-Many: A relationship where each record in one table can be associated with multiple records in another table.
        Many-to-Many: A relationship where records in both tables can be associated with multiple records in the other table.
        Establishing proper relationships between tables helps organize and structure the data effectively.
        
        JOIN Statements
        JOIN statements are used to combine rows from different tables based on related columns. Here are the main types of JOINs:
        
        INNER JOIN: Retrieves rows that have matching values in both tables being joined.
        LEFT JOIN: Retrieves all rows from the left table and matching rows from the right table (if any).
        RIGHT JOIN: Retrieves all rows from the right table and matching rows from the left table (if any).
        FULL JOIN: Retrieves all rows from both tables, including matching and non-matching rows.
        JOIN statements allow you to fetch data from multiple tables, leveraging the relationships established between them.
        
        Aggregation and Grouping
        Aggregation functions in SQL, such as SUM, AVG, COUNT, and others, enable you to summarize and calculate values from a set of rows. The GROUP BY clause is used in conjunction with these functions to group rows based on one or more columns.
        
        Aggregating Data
        Aggregate functions perform calculations on a set of rows and return a single result. For example:
        
        SUM: Calculates the sum of a column's values.
        AVG: Calculates the average of a column's values.
        COUNT: Returns the number of rows in a group.
        MIN: Retrieves the minimum value from a column.
        MAX: Retrieves the maximum value from a column.
        These functions allow you to derive meaningful insights and statistical calculations from your data.
        
        Grouping Data
        The GROUP BY clause is used to group rows based on one or more columns. It allows you to divide the data into logical subsets and apply aggregate functions to each group individually. For example:
        
        SELECT column1, aggregate_function(column2)
        FROM table_name
        GROUP BY column1;
        This statement groups the rows based on column1 and applies the aggregate function to each group.
        
        Subqueries and Views
        SQL subqueries provide a way to nest one query inside another. They can be used to create more complex queries and retrieve data from multiple tables simultaneously.
        
        Views, on the other hand, are virtual tables based on the result of a query. They simplify data retrieval by providing a predefined query that can be treated as a table.
        
        Subqueries
        A subquery is a query embedded within another query. It can be used in the WHERE or FROM clause of the outer query to retrieve data based on intermediate results. Subqueries allow you to break down complex problems into smaller, more manageable parts.
        
        Views
        Views are saved queries that act as virtual tables. They can be created using a SELECT statement and provide an abstraction layer over the underlying tables. Views simplify data retrieval by encapsulating complex queries into a single, reusable entity.
        
        This section has covered the concept of relationships in databases, JOIN statements to retrieve related data, aggregation functions and the GROUP BY clause for summarizing data, and the usage of subqueries and views to handle complex queries.
        
        By understanding these concepts, you'll be able to work with more advanced SQL queries, manipulate data effectively, and gain valuable insights from your databases.
        
        Indexing and Performance Optimization
        Indexes play a crucial role in enhancing the performance of SQL queries by improving data retrieval speed. Understanding how to create and use indexes effectively is essential for optimizing database performance.
        
        Importance of Indexes
        Indexes are data structures that provide quick access to specific data within a table. They enable the database engine to locate data faster by reducing the number of rows that need to be scanned. Indexes are created on one or more columns and significantly enhance query performance, especially for large tables.
        
        Creating Indexes
        To create an index, you need to identify the columns that are frequently used in search conditions or join operations. Using the CREATE INDEX statement, you can specify the index name, the table on which the index will be created, and the column(s) to be indexed. For example:
        
        CREATE INDEX idx_name ON table_name (column1, column2);
        Creating indexes on appropriate columns can significantly speed up query execution.
        
        Using Indexes Effectively
        While indexes boost performance, they also come with some overhead. It's essential to strike a balance between the number of indexes and their impact on data modification operations (inserts, updates, and deletes). Remember to update indexes when modifying data to ensure their accuracy.
        
        Regularly analyze query performance, monitor index usage, and consider adding or removing indexes based on actual usage patterns. Proper indexing strategy is crucial for optimizing database performance.
        
        Transactions and Concurrency Control
        In a multi-user database environment, transactions ensure data integrity and maintain consistency. Understanding transactions and concurrency control is vital when dealing with concurrent database operations.
        
        Transactions and ACID Properties
        A transaction is a logical unit of work that consists of one or more database operations. Transactions adhere to the ACID properties:
        
        Atomicity: A transaction is treated as a single, indivisible unit of work. Either all operations within a transaction are committed, or none of them are.
        Consistency: Transactions bring the database from one consistent state to another consistent state. The integrity of the data is maintained.
        Isolation: Concurrently executing transactions are isolated from each other, ensuring that the intermediate states of transactions are not visible to other transactions.
        Durability: Once a transaction is committed, its changes are permanently saved and can survive system failures.
        Understanding the ACID properties helps ensure data integrity and reliability in database operations.
        
        Isolation Levels
        Isolation levels define the degree of isolation and concurrency control in database transactions. They determine how transactions interact with each other and impact data consistency.
        
        Common isolation levels include:
        
        Read Uncommitted: Allows dirty reads and has the lowest level of isolation.
        Read Committed: Prevents dirty reads, but non-repeatable reads and phantom reads are possible.
        Repeatable Read: Guarantees consistent reads within a transaction, but phantom reads may occur.
        Serializable: Provides the highest level of isolation, ensuring that transactions are executed as if they were processed sequentially.
        Understanding isolation levels helps manage concurrent transactions and maintain data consistency.
        
        Advanced Topics
        SQL offers advanced features that extend its capabilities beyond simple queries. Exploring these advanced topics opens up new possibilities for efficient data management and automation.
        
        Stored Procedures
        Stored procedures are precompiled SQL code that can be stored and executed on the database server. They encapsulate a set of SQL statements as a single unit, enabling code reuse, improved performance, and enhanced security. Stored procedures can accept input parameters and return output values.
        
        Triggers
        Triggers are special SQL constructs that automatically execute in response to specific database events, such as INSERT, UPDATE,
        
        or DELETE operations on tables. Triggers enable you to enforce business rules, maintain data integrity, and automate complex database actions.
        
        User-Defined Functions
        User-defined functions (UDFs) allow you to extend SQL by creating custom functions. UDFs encapsulate specific logic and can be used within SQL statements just like built-in functions. They provide a way to modularize complex calculations or data transformations, improving code readability and reusability.
        
        Exploring these advanced topics will expand your SQL skills and empower you to build more sophisticated database solutions.
        
        Keep learning, practicing, and experimenting with SQL to become proficient in handling diverse data scenarios.
        
        Best Practices
        Writing efficient and maintainable SQL code is essential for building robust and scalable database applications. Here are some best practices to follow:
        
        Naming Conventions
        Use descriptive names for tables, columns, and other database objects. Choose names that accurately represent the data they store or the purpose they serve. Consistent and meaningful naming conventions improve code readability and maintainability.
        
        Code Formatting
        Consistent code formatting enhances readability and makes it easier to understand SQL statements. Indentation, proper spacing, and line breaks improve code structure and organization. Consider using a code formatter or adhering to a style guide for consistent formatting.
        
        Error Handling
        Implement error handling mechanisms in your SQL code to gracefully handle unexpected scenarios. Use structured error handling constructs provided by your database system, such as TRY-CATCH blocks, to catch and handle errors effectively. Proper error handling improves code reliability and maintainability.
        
        Recommended Learning Resources
        To further enhance your SQL skills, explore these recommended learning resources:
        
        Books: ""SQL Cookbook"" by Anthony Molinaro, ""SQL Queries for Mere Mortals"" by John Viescas and Michael J. Hernandez.
        Online Tutorials: SQL tutorials on websites like W3Schools, SQLZoo, and Mode Analytics.
        Video Courses: Online platforms like Udemy, Coursera, and Pluralsight offer SQL courses for beginners.
        Interactive Websites: SQLFiddle, HackerRank, and LeetCode provide interactive SQL challenges and exercises.
        These resources provide comprehensive explanations, hands-on practice, and real-world examples to deepen your SQL knowledge.
        
        Great! Here's an updated version of the ""Exercises and Solutions"" section that includes the mentioned websites:
        
        Exercises and Solutions
        Practice is key to mastering SQL. This repository includes a set of SQL exercises designed specifically for beginners. Each exercise is accompanied by a solution to help you validate your approach and learn from different perspectives.
        
        These exercises cover a range of SQL topics, including querying, data manipulation, joins, and more. Solve the exercises independently, compare your solutions with the provided ones, and explore alternative approaches to strengthen your SQL skills.
        
        Additionally, you can further enhance your SQL proficiency by practicing on dedicated websites that offer SQL exercises and challenges. Check out the following platforms:
        
        SQLZoo: SQLZoo provides interactive SQL exercises and tutorials for beginners. It covers various SQL topics and offers a hands-on learning experience.
        
        HackerRank: HackerRank offers a section dedicated to SQL exercises. Solve SQL problems and test your skills on their platform.
        
        LeetCode: LeetCode, known for coding challenges, also offers a database section with SQL exercises. Practice SQL problems and improve your problem-solving abilities.
        
        Challenge yourself with these exercises and watch your SQL proficiency grow!
        
        Conclusion
        Congratulations on completing SQL 101: Beginner's Guide to SQL Programming! You've learned essential SQL concepts, from querying data to optimizing performance. Remember best practices, practice with exercises, and explore recommended resources.
        
        Good luck and Happy coding with SQL!";

    public const string LongSourceTextDE =
        @"Das Byzantinische Reich (auch Oströmisches Reich oder kurz Byzanz bzw. Ostrom) war die unmittelbare Fortsetzung des Römischen Reiches im östlichen Mittelmeerraum. Das Reich ging aus der Reichsteilung von 395 hervor und existierte bis zur Eroberung Konstantinopels durch die Osmanen im Jahr 1453, womit es den Untergang Westroms um fast 1000 Jahre überdauerte.

Die Bezeichnung (die sich von Byzanz, dem ursprünglichen Namen der Hauptstadt Konstantinopel, herleitet) ist modernen Ursprungs. In der Spätantike und im Mittelalter lautete die Eigenbezeichnung Βασιλεία τῶν Ῥωμαίων (Basileia tōn Rhōmaiōn „Reich der Römer“). Zur Zeit seiner größten Ausdehnung in der Mitte des sechsten Jahrhunderts schloss das Reich zurückeroberte Provinzen des untergegangenen Westreichs ein und erstreckte sich von Südspanien über Italien, die Balkanhalbinsel, Kleinasien und Syrien bis nach Ägypten und über weitere Teile Nordafrikas. Seit dem siebten Jahrhundert weitgehend auf Kleinasien und Südosteuropa beschränkt, war es während der längsten Zeit seiner Existenz dennoch das mächtigste, reichste und kulturell bedeutendste Staatswesen der Christenheit.

Die Geschichte des Reiches war vom Abwehrkampf gegen äußere Feinde geprägt, der seine Kräfte erheblich beanspruchte und seine Ressourcen in der Spätzeit erschöpfte. Zuvor wechselten sich Phasen des Rückzugs, etwa nach den Gebietsverlusten im siebten Jahrhundert, mit solchen der Expansion ab, wie während der Eroberungen im zehnten und elften Jahrhundert. Im Inneren kam es immer wieder zu unterschiedlich stark ausgeprägten theologischen Auseinandersetzungen sowie zu vereinzelten Bürgerkriegen, doch blieb das an römischen Strukturen orientierte staatliche Fundament bis ins frühe 13. Jahrhundert weitgehend intakt.

Da die Christianisierung Osteuropas wesentlich von Byzanz ausging, übte es einen starken Einfluss auf Kunst und Kultur der Länder der Orthodoxie aus. Dies betrifft insbesondere Griechenland und Russland, die sich zeitweilig als Nachfolger Ostroms betrachteten. Aber auch für Westeuropa ist der byzantinische Einfluss von großer Bedeutung. Da das Erbe der Antike in Byzanz stärker bewahrt wurde, nahm das Reich vor und während der Renaissance eine wichtige kulturelle Mittlerrolle ein. Bedeutende Werke der Antike, etwa des Rechts oder der Literatur sind dem Westen durch byzantinische Gelehrte überliefert worden.

Begriffsbestimmung und Begriffsgeschichte

Staatswappen des Byzantinischen Reiches unter den Palaiologen: Der Doppeladler symbolisierte den Herrschaftsanspruch des christlich-römischen Kaisers über beide Reichshälften.
Der Byzantinist Georg Ostrogorsky charakterisierte das Byzantinische Reich als eine Mischung aus römischem Staatswesen, griechischer Kultur und christlichem Glauben.[1]

In der modernen Forschung wird die Geschichte des Byzantinischen Reiches in drei Phasen unterteilt:

die spätantik-frühbyzantinische Zeit (um 300 bis Mitte des 7. Jahrhunderts), in der das Reich als Osthälfte des Imperium Romanum noch antik-römisch geprägt war und als intakte Großmacht den gesamten östlichen Mittelmeerraum kontrollierte;
die mittelbyzantinische Zeit (Mitte des 7. Jahrhunderts bis 1204/1261), in der sich das nun vollkommen gräzisierte Reich nach großen Gebietsverlusten wieder konsolidierte und immer noch ein bedeutender Machtfaktor im Mittelmeer war;
die spätbyzantinische Zeit (1204/1261 bis 1453), in der das Reich auf einen Stadtstaat zusammenschrumpfte und in der Region politisch keine Rolle mehr spielte.
Neben dieser traditionellen Periodisierung existieren auch teils davon abweichende Überlegungen; so setzt sich in der neueren Forschung zunehmend die Tendenz durch, die im engeren Sinne „byzantinische“ Geschichte erst mit dem späten sechsten oder dem siebten Jahrhundert beginnen zu lassen und die Zeit davor der (spät-)römischen Geschichte zuzurechnen.[2] Zwar ist diese Position nicht unumstritten,[3] doch in der Praxis beschäftigten sich mit der oströmischen Geschichte vor dem frühen 7. Jahrhundert heute in der Tat vor allem Althistoriker, während sich die meisten Byzantinisten inzwischen auf die Folgezeit konzentrieren.

Die von der Hauptstadt abgeleiteten Bezeichnungen Byzantiner und Byzantinisches Reich sind modernen Ursprungs. Einige Byzantinisten wie Anthony Kaldellis plädieren daher in jüngerer Zeit dafür, den Terminus Byzanz zugunsten von Ostrom gänzlich aufzugeben, da es keinen relevanten Kontinuitätsbruch zwischen Antike und Mittelalter gegeben habe.[4] Die Byzantiner – und die Griechen bis ins 19. Jahrhundert hinein – betrachteten und bezeichneten sich selbst als „Römer“ (Ῥωμαῖοι Rhōmaîoi; vergleiche Rhomäer). Das Wort „Griechen“ (Ἕλληνες Héllēnes/Éllines) wurde fast nur für die vorchristlichen, paganen griechischen Kulturen und Staaten verwendet. Erst um 1400 bezeichneten sich auch einige gebildete Byzantiner wie Georgios Gemistos Plethon als „Hellenen“. Zeitgenossen sprachen stets von der Βασιλεία τῶν Ῥωμαίων (Basileía tōn Rhōmaíōn/Vasilía ton Romäon „Reich der Römer“) oder der Ῥωμαϊκὴ Αὐτοκρατορία (Rhōmaïkḗ Autokratoría/Romaikí Aftokratoría „Römischer Herrschaftsbereich“ bzw. „Römisches Kaiserreich“; dies ist die direkte Übersetzung des lateinischen Imperium Romanum ins Griechische). Nach ihrem Selbstverständnis waren sie also nicht die Nachfolger des Römischen Reiches – sie waren das Römische Reich. Deutlich wird dies auch dadurch, dass die Bezeichnungen „Oströmisches“ und „Weströmisches Reich“ modernen Ursprungs sind und es nach zeitgenössischer Auffassung nur ein Reich unter zwei Kaisern gab, solange beide Reichsteile existierten.

Formal war dieser Anspruch berechtigt, da es im Osten keinen Einschnitt wie im Westen gegeben hatte und Byzanz in einem weitaus nahtloser an die Spätantike anschließenden Zustand fortbestand, der sich erst nach und nach veränderte und zu einer Gräzisierung des Staates unter Herakleios führte. Allerdings war bereits vorher die vorherrschende Identität des Oströmischen Reiches griechisch und Latein nur die Sprache der Herrschaft gewesen, die in der Armee, am Hof und in der Verwaltung benutzt wurde, nicht im Alltag. Altgriechisch und seit der Wende um 600 das Mittelgriechische, lautlich mit dem heutigen Griechisch schon fast identisch, ersetzte nicht nur seit Herakleios Latein als Amtssprache, sondern war auch die Sprache der Kirche, Literatursprache (bzw. Kultursprache) und Handelssprache.

Das Oströmische/Byzantinische Reich verlor seinen römisch-spätantiken Charakter erst im Laufe der arabischen Eroberungen im siebten Jahrhundert. Es sah sich zeit seines Bestehens als unmittelbar und einzig legitimes, weiterbestehendes Römisches Kaiserreich und leitete daraus einen Anspruch auf Oberhoheit über alle christlichen Staaten des Mittelalters ab. Dieser Anspruch war zwar spätestens seit dem 7. Jahrhundert nicht mehr durchsetzbar, wurde aber in der Staatstheorie konsequent aufrechterhalten.

Politische Geschichte
Die Spätantike: Das Oströmische Reich
→ Hauptartikel: Spätantike

Kopf einer zeitgenössischen Kolossalstatue Konstantins I. (Kapitolinische Museen, Rom)
Die Reichsteilungen seit Konstantin dem Großen
Die Wurzeln des Byzantinischen Reiches liegen in der römischen Spätantike (284–641). Das Byzantinische Reich stellte keine Neugründung dar, vielmehr handelt es sich um die bis 1453 weiter existierende östliche Hälfte des 395 endgültig geteilten Römerreichs, also um die direkte Fortsetzung des Imperium Romanum. Die damit verbundene Frage, wann die byzantinische Geschichte konkret beginnt, ist allerdings nicht eindeutig zu beantworten, da verschiedene Forschungsansätze möglich sind. Vor allem in der älteren Forschung wurde als Beginn oft die Regierungszeit Kaiser Konstantins des Großen (306 bis 337) angesehen, während in der neueren Forschung die Tendenz vorherrscht, erst die Zeit ab dem 7. Jahrhundert als „byzantinisch“ und die davor liegende Zeit noch als eindeutig zur Spätantike gehörig zu charakterisieren, wenngleich auch dies nicht unumstritten ist.[5]

Konstantin setzte sich in einem von 306 bis 324 dauernden Machtkampf im Imperium als Alleinherrscher durch (im Westen bereits seit 312), reformierte Heer und Verwaltung und festigte das Reich nach außen. Er begünstigte als erster römischer Kaiser aktiv das Christentum (konstantinische Wende), was enorme Auswirkungen hatte; zum anderen schuf er die spätere Hauptstadt des Byzantinischen Reiches. Zwischen 325 und 330 ließ er die alte griechische Polis Byzanz großzügig ausbauen und benannte sie nach sich selbst in Konstantinopel um (allerdings blieb auch Byzantion als alternativer Name der Stadt gebräuchlich). Bereits zuvor hatten sich Kaiser Residenzen gesucht, die näher an den bedrohten Reichsgrenzen lagen und/oder besser zu verteidigen waren als Rom, das spätestens nach der kurzen Herrschaft des Kaisers Maxentius in der Regel nicht mehr Sitz der Kaiser, sondern nur noch ideelle Hauptstadt war. Allerdings erhielt Konstantinopel im Unterschied zu anderen Residenzstädten einen eigenen Senat, der unter Konstantins Sohn Constantius II. dem römischen formal gleichgestellt wurde. Mehr und mehr entwickelte sich die Stadt in der Folgezeit zum verwaltungsmäßigen Schwerpunkt des östlichen Reichsteils. Gegen Ende des 4. Jahrhunderts kamen sogar die Bezeichnungen Nova Roma und Νέα Ῥώμη (Néa Rhṓmē) auf – das „Neue Rom“. Trotz dieses bewussten Gegensatzes zur alten Hauptstadt blieb das alte Rom weiterhin der Bezugspunkt der Reichsideologie. Seit der Zeit des Kaisers Theodosius I. war Konstantinopel dann die dauerhafte Residenz der im Osten regierenden römischen Kaiser.


Das von Konstantin eingeführte Labarum mit dem Christusmonogramm
Nach Konstantins Tod 337 gab es zumeist mehrere Augusti im Imperium, denen die Herrschaft über bestimmte Reichsteile oblag. Dabei wurde allerdings zugleich die Einheit des Imperium Romanum nie in Frage gestellt, vielmehr handelte es sich um ein Mehrkaisertum mit regionaler Aufgabenteilung, wie es seit Diokletian üblich geworden war. Den Osten regierten Constantius II. (337 bis 361), Valens (364 bis 378) und Theodosius I. (379 bis 395). Nach dem Tod des Theodosius, der 394/395 als letzter Kaiser kurzzeitig faktisch über das gesamte Imperium herrschte, wurde das Römische Reich 395 erneut in eine östliche und eine westliche Hälfte unter seinen beiden Söhnen Honorius und Arcadius aufgeteilt. Solche „Reichsteilungen“ hatte es zwar schon oft gegeben, aber diesmal erwies sie sich als endgültig: Arcadius, der in Konstantinopel residierte, gilt daher manchen Forschern als erster Kaiser des Oströmischen beziehungsweise Frühbyzantinischen Reiches. Dennoch galten weiterhin alle Gesetze in beiden Reichshälften (sie wurden meist im Namen beider Kaiser erlassen), und der Konsul des jeweils anderen Teiles wurde anerkannt. Umgekehrt rivalisierten beide Kaiserhöfe während des fünften Jahrhunderts um den Vorrang im Gesamtreich.


Die Reichsteilung von 395
Im späten vierten Jahrhundert, zur Zeit der beginnenden sogenannten Völkerwanderung,[6] war zunächst die östliche Reichshälfte Ziel germanischer Kriegerverbände wie der West- und der Ostgoten. In der Schlacht von Adrianopel erlitt das oströmische Heer 378 eine schwere Niederlage gegen meuternde (West-)Goten, denen dann 382 von Theodosius I. südlich der Donau als formal reichsfremde Foederati Land zugewiesen wurde. Seit Beginn des fünften Jahrhunderts richteten sich die äußeren Angriffe dann aber zunehmend auf das militärisch und finanziell schwächere Westreich, das zugleich in endlosen Bürgerkriegen versank, die zu einem langsamen Zerfall führten. Ob den germanischen Kriegern eine entscheidende Rolle beim Untergang Westroms zukam, ist in der neueren Forschung sehr umstritten.[7] Im Osten konnte hingegen weitgehende innenpolitische Stabilität bewahrt werden. Nur vereinzelt musste sich Ostrom der Angriffe des neupersischen Sassanidenreichs erwehren, des einzigen gleichrangigen Konkurrenten Roms, mit dem aber zwischen 387 und 502 fast durchgängig Frieden herrschte. 410 wurde die Stadt Rom von meuternden westgotischen foederati geplündert, was auch im Osten eine deutliche Schockwirkung auf die Römer hatte, während die östliche Reichshälfte, abgesehen vom Balkanraum, den wiederholt Kriegerverbände durchzogen, weitgehend unbehelligt blieb und vor allem den inneren Frieden (pax Augusta) alles in allem wahren konnte. Ostrom versuchte durchaus, die Westhälfte zu stabilisieren, und intervenierte wiederholt mit Geld und Truppen. So wurde die erfolglose Flottenexpedition gegen die Vandalen 467/468 (siehe Vandalenfeldzug) wesentlich von Ostrom getragen. Doch letztlich war der Osten zu sehr mit der eigenen Konsolidierung beschäftigt, um den Verfall des Westreichs aufhalten zu können.

Das Oströmische Reich nach dem Untergang des Westreichs
Im späteren fünften Jahrhundert hatte auch das Ostreich mit schweren Problemen zu kämpfen. Einige politisch bedeutsame Positionen wurden von Soldaten, nicht selten Männer „barbarischer“ Herkunft, dominiert (insbesondere in Gestalt des magister militum Aspar), die immer unbeliebter wurden: Es drohte die Gefahr, dass auch in Ostrom, so wie es bereits zuvor im Westen geschehen war, die Kaiser und die zivile Administration dauerhaft unter die Vorherrschaft mächtiger Militärs geraten würden. Unter Kaiser Leo I. (457–474) versuchte man daher, die vor allem aus foederati bestehende Gefolgschaft Aspars zu neutralisieren, indem man gegen sie insbesondere Isaurier, die Bewohner der Berge Südostkleinasiens waren, also Reichsangehörige, ausspielte. Leo stellte zudem eine neue kaiserliche Leibgarde auf, die excubitores, die dem Herrscher persönlich treu ergeben waren; auch unter ihnen fanden sich viele Isaurier. In Gestalt von Zeno konnte einer von ihnen 474 sogar den Kaiserthron besteigen, nachdem Aspar 471 ermordet worden war. Auf diese Weise gelang es den Kaisern zwischen 470 und 500 schrittweise, das Militär wieder unter Kontrolle zu bringen. Denn unter Kaiser Anastasios I. konnte dann bis 498 auch der gewachsene Einfluss der Isaurier unter großen Kraftanstrengungen wieder zurückgedrängt werden. In der neueren Forschung wird die Ansicht vertreten, dass die Ethnizität der Beteiligten bei diesem Machtkampf in Wahrheit eine untergeordnete Rolle gespielt habe: Es sei nicht etwa um einen Konflikt zwischen „Barbaren“ und „Römern“, sondern vielmehr um ein Ringen zwischen dem kaiserlichen Hof und der Armeeführung gegangen, in dem sich die Kaiser zuletzt durchsetzen konnten.[8] Das Heer blieb zwar auch weiterhin von auswärtigen, oft germanischen, Söldnern geprägt; der Einfluss der Feldherren auf die Politik war fortan allerdings begrenzt, und die Kaiser gewannen wieder stark an Handlungsfreiheit.

Etwa zur gleichen Zeit endete im Westen das Kaisertum, das bereits im späten 4. Jahrhundert gegenüber den hohen Militärs zunehmend an Macht eingebüßt hatte, wodurch die letzten Westkaiser faktisch kaum noch selbstständig herrschten. Hinzu kam im 5. Jahrhundert der sukzessive Verlust der wichtigsten westlichen Provinzen (vor allem der reichen Provinzen Africa und Gallien) an die neuen germanischen Herrscher, was einen nicht mehr kompensierbaren Verlust an Finanzmitteln und damit eine Erosion der weströmischen Staatsgewalt bedeutete. Der machtlose letzte weströmische Kaiser Romulus Augustulus wurde im Jahr 476 von dem Heerführer Odoaker abgesetzt (der letzte von Ostrom anerkannte Kaiser war allerdings Julius Nepos, der 480 in Dalmatien ermordet wurde).[9] Odoaker unterstellte sich dem Ostkaiser. Dieser war fortan de iure wieder alleiniger Herr über das Gesamtreich, wenngleich die Westgebiete faktisch verloren waren. Die meisten Reiche, die sich nun unter Führung von nichtrömischen reges auf den Trümmern des zerfallenen Westreichs bildeten, erkannten den (ost-)römischen Kaiser aber lange Zeit zumindest als ihren nominellen Oberherrn an. Kaiser Anastasios I. stärkte um die Wende zum sechsten Jahrhundert auch die Finanzkraft des Reiches, was der späteren Expansionspolitik Ostroms zugutekam.

Siehe auch: Byzantinisches Reich unter der Leoniden-Dynastie
Das Zeitalter Justinians

Mosaikbild aus dem Altarraum von San Vitale in Ravenna, um 545. Die zu einer Gruppe von spätantiken Kaiserbildern gehörende Darstellung zeigt den amtierenden Kaiser Justinian mit seinem Gefolge.[10]
Im sechsten Jahrhundert eroberten unter Kaiser Justinian (527–565) die beiden oströmischen Feldherren Belisar und Narses große Teile der weströmischen Provinzen – Italien, Nordafrika und Südspanien – zurück und stellten damit das Imperium Romanum für kurze Zeit in verkleinertem Umfang wieder her. Doch die Kriege gegen die Reiche der Vandalen und Goten im Westen und gegen das mächtige Sassanidenreich unter Chosrau I. im Osten, sowie ein Ausbruch der Pest, die ab 541 die ganze Mittelmeerwelt heimsuchte, zehrten erheblich an der Substanz des Reiches.[11] Während der Regierungszeit Justinians, der als letzter Augustus Latein zur Muttersprache hatte, wurde auch die Hagia Sophia erbaut, für lange Zeit die größte Kirche der Christenheit und der letzte große Bau des Altertums. Ebenso kam es 534 zur umfassenden und wirkmächtigen Kodifikation des römischen Rechts (das später so genannte Corpus iuris civilis). Auf dem religionspolitischen Sektor konnte der Kaiser trotz großer Anstrengungen keine durchschlagenden Erfolge erzielen. Die andauernden Spannungen zwischen orthodoxen und monophysitischen Christen stellten neben der leeren Staatskasse, die Justinian hinterließ, eine schwere Hypothek für seine Nachfolger dar. Justinians lange Herrschaft markiert eine wichtige Übergangszeit vom spätantiken zum mittelbyzantinischen Staat, auch wenn man Justinian, den „letzten römischen Imperator“ (Georg Ostrogorsky), insgesamt sicherlich noch zur Antike zu zählen hat. Unter seinen Nachfolgern nahm dann auch die Bedeutung und Verbreitung der lateinischen Sprache im Reich immer weiter ab, und Kaiser Maurikios gab mit der Einrichtung der Exarchate in Karthago und Ravenna erstmals den spätantiken Grundsatz der Trennung von zivilen und militärischen Kompetenzen auf, wenngleich er im Kerngebiet des Reiches noch an der herkömmlichen Verwaltungsform festhielt.


Das Restaurationswerk Justinians I. (527–565)
Ab der zweiten Hälfte des sechsten Jahrhunderts brachten leere Kassen und an allen Fronten auftauchende Feinde das Reich erneut in ernste Schwierigkeiten. In der Regierungszeit von Justinians Nachfolger Justin II., der 572 einen Krieg mit Persien provozierte, infolge seiner Niederlage einen Nervenzusammenbruch erlitt und dem Wahnsinn verfiel, besetzten die Langobarden bereits ab 568 große Teile von Italien. Währenddessen drangen die Slawen seit etwa 580 in den Balkanraum ein und besiedelten ihn bis zum Ende des siebten Jahrhunderts größtenteils. Mit dem gewaltsamen Tod des Kaisers Maurikios im Jahr 602, der 591 einen vorteilhaften Frieden mit den Sassaniden hatte schließen können und energisch gegen die Slawen vorgegangen war, eskalierte die militärische Krise.

Die Dynastie des Herakleios und der Übergang zum Byzantinischen Reich
Maurikios war der erste oströmische Kaiser, der einem Usurpator erlag, und seinem übel beleumundeten Nachfolger Phokas gelang es nicht, die Stellung des Monarchen wieder zu stabilisieren. Seit 603 erlangten zudem die sassanidischen Perser unter Großkönig Chosrau II. zeitweilig die Herrschaft über die meisten östlichen Provinzen.[12] Bis 619 hatten sie Ägypten und Syrien (die reichsten oströmischen Provinzen) erobert und standen 626 sogar vor Konstantinopel. Ostrom schien am Rande des Untergangs zu stehen, da auf dem Balkan auch die Awaren und ihre slawischen Untertanen auf kaiserliches Gebiet vordrangen. Begünstigt wurden diese Vorgänge noch durch einen Bürgerkrieg zwischen Kaiser Phokas und seinem Rivalen Herakleios. Letzterer konnte sich im Jahr 610 durchsetzen und nach hartem Kampf auch die Wende im Krieg gegen die Perser herbeiführen: In mehreren Feldzügen drang er seit 622 auf persisches Gebiet vor und schlug ein sassanidisches Heer Ende 627 in der Schlacht bei Ninive. Zwar waren die Sassaniden militärisch nicht entscheidend besiegt worden, aber Persien war nun auch an anderen Fronten bedroht und wünschte daher Ruhe im Westen. Der unbeliebte Chosrau II. wurde gestürzt, und sein Nachfolger schloss Frieden mit Ostrom. Persien räumte die eroberten Gebiete und versank aufgrund interner Machtkämpfe bald im Chaos. Nach dieser gewaltigen Anstrengung waren die Kräfte des Oströmischen Reichs jedoch erschöpft. Die Senatsaristokratie, die ein wesentlicher Träger der spätantiken Traditionen gewesen war, war zudem bereits unter Phokas stark geschwächt worden.[13] Die Herrschaft über den größten Teil des Balkans blieb verloren.

Herakleios ließ den Sieg über die Perser und die Rettung des Imperiums dennoch aufwändig feiern und übertrieb dabei wohl seinen Erfolg. Doch der oströmische Triumph war von kurzer Dauer. Der militärischen Expansion der durch ihren neuen muslimischen Glauben angetriebenen Araber, die in den 630er Jahren einsetzte, hatte das Reich nach dem langen und kräftezehrenden Krieg gegen Persien nicht mehr viel entgegenzusetzen. Herakleios musste erleben, wie die eben erst von den Sassaniden geräumten Orientprovinzen erneut verloren gingen, dieses Mal für immer. In der entscheidenden Schlacht am Jarmuk am 20. August 636 unterlagen die Oströmer einem Heer des zweiten Kalifen ʿUmar ibn al-Chattāb, und der ganze Südosten des Reichs, einschließlich Syriens, Ägyptens und Palästinas, ging bis 642 vollständig verloren; bis 698 verlor man auch Africa mit Karthago.[14]


Die Islamische Expansion:
﻿Ausbreitung unter Mohammed, 622–632
﻿Ausbreitung unter den vier „rechtgeleiteten Kalifen“, 632–661
﻿Ausbreitung unter den Umayyaden, 661–750

Die kleinasiatischen Themen um 750
Nach 636 stand Ostrom am Rand des Abgrunds. Im Gegensatz zu seinem langjährigen Rivalen, dem Sassanidenreich, das trotz heftiger Gegenwehr 642/651 unterging, konnte sich das Oströmische bzw. Byzantinische Reich aber immerhin erfolgreich gegen eine vollständige islamische Eroberung verteidigen. Die kaiserlichen Truppen, die bisher die vorderorientalischen Provinzen verteidigt hatten, mussten sich aber nach Kleinasien zurückziehen, das von arabischen Angriffen heimgesucht wurde (Razzien). Im Verlauf des siebten Jahrhunderts verlor Byzanz infolge der islamischen Expansion zeitweilig sogar die Seeherrschaft im östlichen Mittelmeer (Niederlage bei Phoinix 655) und konnte zudem auch Kleinasien nur mit Mühe halten, während auf dem Balkan Slawen und Bulgaren das Reich bedrängten und die kaiserliche Herrschaft hier auf einige wenige Orte begrenzten. So waren die Oströmer um 700 im Wesentlichen auf einen Rumpfstaat mit Kleinasien, dem Umland der Hauptstadt, einiger Gebiete in Griechenland sowie in Italien reduziert. Der Verlust Ägyptens 642 bedeutete den härtesten Schlag für Byzanz, da die hohe Wirtschaftsleistung (Ägypten war die Provinz mit dem höchsten Steueraufkommen) und das Getreide Ägyptens für Konstantinopel essentiell gewesen waren.[15]";
}

