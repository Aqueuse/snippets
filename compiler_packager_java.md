# Compiler et packager un projet Java



*Où on apprend comment compiler du code source écrit en java et produire un jar exécutable qui contiendra également les ressources nécessaires à son fonctionnement.*



C'est bien beau d'écrire du code java et de le tester dans une IDE ou avec VS code, mais ensuite, comment fait-on pour passer du projet à l'exécutable ?



Bien que les IDE proposent toujours des options et des commandes pour le faire à notre place, je leur trouve des tas de problèmes à l'usage et ils rendent la chose plus complexe qu'elle ne l'ait réellement. En plus, on n'a pas toujours la possibilité de compiler de cette manière selon la machine où on se trouve. La ligne de commande reste toujours indispensable dans certains contextes (serveur, système embarqué, petite machine, faible espace disque, etc).



Il n'est pas toujours nécessaire non plus d'utiliser Maven, Gradle ou Ant pour arriver à un résultat acceptable, bien que ces outils permettent déjà de descendre d'un niveau en terme d'abstraction. Maitriser ces outils demandent du temps et des efforts et ça ne se justifie pas toujours pour des projets occasionnels et/ou modestes.



Je vous propose donc ce tutoriel, qui prend appuie sur un petit projet maison, avec tout de même assez de sous dossiers et de fichiers ressources pour dépasser le sempiternelle `javac HelloWorld HelloWord.java` et proposer une solution réaliste en terme d'échelle.



On travaillera avec un dossier de projet classique avec src/packages/classes, le style de dossier de projet qu'on peut produire dans Eclipse ou Intelij par exemple.



Je fais volontairement l'impasse sur les librairies externes de façon à clarifier le tutoriel mais le fonctionnement n'est pas différent que pour des assets.



Pour les curieux, ce projet est un neko, une applet de bureau sous la forme d'un chaton qui poursuit le pointeur de souris :)




## Qu'est-ce qu'un jar en fait ?



Un jar (une **J**ava **AR**chive) est une archive de type zip améliorée, qui contient nos classes, leurs éventuelles ressources (dont les librairies, les assets, etc.) et surtout un dossier `META-INF` à sa racine, avec un manifeste (MANIFEST.MF), qui permet d'exécuter l'archive comme s'il s'agissait d'un simple exécutable.

├── META-INF
│   └── MANIFEST.MF
├── neko
│   └── Neko.class
│   └── images
│       └── mes assets
├── systemTray
│   └── MySystemTray.class
│   └── images
│       └── mes assets
├── toy
│   └── Toy.class
│   └── images
│       └── mes assets

## Préparation des classes :

Pour produire un jar exploitable, il est préconisé, dans notre code source, de se référer aux ressources de façon relative aux classes avec `MyClasse.class.getRessource`.

De cette façon, quand on compilera, le programme trouvera toujours les bons chemins de fichiers, peu importe le répertoire où se trouvera notre jar.

Exemple :

```java
new ImageIcon(Objects.requireNonNull(Neko.class.getResource("images/" + "left1.GIF")))
```

Ici `Neko.class.getRessource` renvoi vers le chemin de la classe Neko, peu importe où il se trouve au moment de l'exécution.

## Compilation des classes :

Pour compiler nos classes java, On descend dans le dossier src et on compile en spécifiant à `javac` les différents fichiers *.java* dans les différents packages destinés à la compilation.

```bash
$ cd src
$ javac.exe .\neko\*.java .\toy\*.java .\systemTray\*.java
```


## Création du jar :

On utilise l'utilitaire `jar` avec l'option *-e package.MyMainClass* de façon à produire un manifeste où sera spécifié la *main class*, la classe principale, le point d'entrée de l'application, dans notre cas **Neko** dans le package *neko*.

Le manifeste contiendra donc ceci :

> Manifest-Version: 1.0
> Created-By: 16.0.1 (Oracle Corporation)
> **Main-Class: neko.Neko**

Et voici la commande à exécuter :

```bash
$ jar cfve ../../neko.jar neko.Neko .\*
```

**-c** permet de créer l'archive jar.
**-f** permet de spécifier le nom et le chemin du jar.
**-v** permet d'avoir une sortie verbeuse, afin d'être sûr du résultat.

Le **.\\*** final permet d'inclure tous nos dossiers et sous dossiers dans le jar, donc nos classes + nos assets dans leurs sous dossiers image.

## On teste l'exécution du jar :

En ligne de commande, exécuter un jar est simple comme bonjour. Java fournit une option *-jar*. Il suffit de spécifier le chemin et le nom du jar et le tour est joué.

```bash
cd ../..
java -jar neko.jar
```
