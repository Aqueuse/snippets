_String selector (masculin singulier) :_
: _usine à gaz en chaine de caractères pour une sélection simplifiée_



# constrain :
Always beetween two objects defined with a name.

* name : name given to the object with the parameter name in the assembly.

* possible selectors : (plurial, lowerCase)
   * vertices,
   * edges,
   * faces,
   * solids,
   * wires

* possible string selectors : https://cadquery.readthedocs.io/en/latest/selectors.html

* possible constrain : (Singular, upperCase)
   * Axis,
   * Point,
   * Plane

**Usage :**

```
constrain(
   [object1's name@selector@string selector],
   [object2's name@selector@string selector],
   [type de contrainte]
)
```


====

# constrain :
S'opère toujours entre deux objets avec un nom.


* nom : nom donné à l'objet avec le paramètre name="" dans l'assembly.

* Sélecteurs possibles : (pluriel, minuscule)
   * vertices,
   * edges,
   * faces,
   * solids,
   * wires

* Contraintes possibles : (singulier, majuscule)
   * Axis,
   * Point,
   * Plane

* Raccourci des paramètres de sélecteurs :
https://cadquery.readthedocs.io/en/latest/selectors.html


***Usage :***

```
constrain(
    [nom de l'objet1@selecteur@ type de selecteur],  
    [nom de l'objet2@selecteur@ type de selecteur], 
    [type de contrainte]
)
```



