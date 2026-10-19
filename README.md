# Universidad de La Laguna (ULL)
## Grado en Ingeniería Informática - Interfaces Inteligentes
### Práctica 4: Eventos y Patrón Observador en Unity

En esta práctica hemos trabajado la comunicación entre objetos dentro de Unity aplicando el **Patrón Observador** mediante el uso de **delegados (`delegate`)** y **eventos (`event`)** en C#. De esta forma conseguimos que los objetos reaccionen a colisiones o aproximaciones sin tener que estar comprobando condiciones constantemente en el `Update()`.

---

## Ejercicio 1: Esferas por grupos y colisión con el Cilindro

### Explicación del ejercicio
Creamos una escena con 5 esferas divididas en dos grupos mediante etiquetas (`Tipo1` de color rojo y `Tipo2` de color verde), además de un cubo controlado por el jugador y un cilindro que actúa como notificador. Cuando el cubo choca con el cilindro, este lanza un evento: las esferas rojas se dirigen hacia una esfera verde fijada previamente en el Inspector, mientras que las esferas verdes se desplazan hacia el cilindro.

### Cómo lo hemos resuelto y funciones clave
- **`delegate` y `event` (`01_CilindroNotificador.cs`)**: Definimos `public delegate void MensajeColision()` y el evento `OnColisionCubo`. Cuando el cilindro detecta al jugador mediante `OnCollisionEnter` u `OnTriggerEnter` (comprobando `CompareTag("Player")`), invoca el evento.
- **Suscripción y Callbacks (`01_EsferaObservador.cs`)**: En el `Start()` de las esferas comprobamos su etiqueta con `CompareTag("Tipo1")` o `CompareTag("Tipo2")`, les asignamos su color a través de `GetComponent<Renderer>().material.color` y las suscribimos con el operador `+=` a `RespuestaTipo1` o `RespuestaTipo2`.
- **Movimiento (`01_MovimientoCubo.cs` y `01_EsferaObservador.cs`)**: En el `Update()` usamos `Vector3.MoveTowards` manteniendo la coordenada `Y` propia para desplazar cada esfera hacia su destino correspondiente. También liberamos la suscripción en `OnDestroy()` con `-=`.

### Scripts del ejercicio
- [01_CilindroNotificador.cs](https://github.com/guillrl1/P04_II_Delegados_Eventos/blob/main/Scripts/01_CilindroNotificador.cs)
- [01_EsferaObservador.cs](https://github.com/guillrl1/P04_II_Delegados_Eventos/blob/main/Scripts/01_EsferaObservador.cs)
- [01_MovimientoCubo.cs](https://github.com/guillrl1/P04_II_Delegados_Eventos/blob/main/Scripts/01_MovimientoCubo.cs)

### Demostración
![Demostración Ejercicio 1](https://github.com/guillrl1/P04_II_Delegados_Eventos/blob/main/P4_gifs/01.gif)

---

## Ejercicio 2: Sustitución de objetos geométricos por Humanoides

### Explicación del ejercicio
Sustituimos las esferas geométricas del ejercicio anterior por modelos 3D humanoides de la Asset Store (*Mini Legion Footman* para el `Tipo1` y *Mini Legion Lich* para el `Tipo2`), manteniendo la mecánica de notificación pero haciendo que los personajes se orienten hacia el objetivo mientras caminan.

### Cómo lo hemos resuelto y funciones clave
- **Adaptación de materiales a URP**: Convertimos los materiales de los paquetes importados al *Universal Render Pipeline* (`Universal Render Pipeline/Lit`) y reasignamos sus texturas Albedo en el canal *Base Map*.
- **Orientación hacia el destino (`02_HumanoideObservador.cs`)**: Añadimos `transform.LookAt(posicionDestino)` justo antes de `Vector3.MoveTowards` para que los soldados y los magos roten en dirección al objetivo al que persiguen, además de controlar una `distanciaParada` con `Vector3.Distance` para evitar que los modelos se solapen al llegar.

### Scripts del ejercicio
- [02_HumanoideObservador.cs](https://github.com/guillrl1/P04_II_Delegados_Eventos/blob/main/Scripts/02_HumanoideObservador.cs)

### Demostración
![Demostración Ejercicio 2](https://github.com/guillrl1/P04_II_Delegados_Eventos/blob/main/P4_gifs/02.gif)

---

## Ejercicio 3: Interacción entre Humanoides y Escudos (Tipo 1 y Tipo 2)

### Explicación del ejercicio
Ampliamos la escena añadiendo prefabs de escudos de dos tipos (`Escudo1` y `Escudo2`, siendo estos últimos objetos físicos con `Rigidbody`). Ahora el propio cubo actúa como notificador al chocar con los humanoides:
- Si el cubo colisiona con un humanoide `Tipo2`, los del grupo `Tipo1` se dirigen a un escudo seleccionado de antemano.
- Si el cubo toca a un humanoide `Tipo1`, se dirigen hacia los escudos físicos de `Tipo2`.
- Si cualquier humanoide choca con un escudo, cambia el color de su malla.

### Cómo lo hemos resuelto y funciones clave
- **Eventos estáticos (`03_CuboNotificador.cs`)**: Declaramos `public static event MensajeColision OnColisionHumanoideTipo1` y `OnColisionHumanoideTipo2` para que todos los humanoides puedan suscribirse directamente a través de la clase `CuboNotificador` sin necesidad de arrastrar referencias individuales.
- **Búsqueda dinámica de escudos (`03_Perseguidor.cs`)**: Para ir hacia los escudos del grupo 2 usamos `GameObject.FindGameObjectsWithTag("Escudo2")` y calculamos cuál es el más cercano con `Vector3.Distance`.
- **Cambio de color en mallas con esqueleto**: Como en los modelos animados la malla está en un objeto hijo (`SkinnedMeshRenderer`), usamos `GetComponentInChildren<Renderer>().material.color` al detectar el impacto con el escudo.

### Scripts del ejercicio
- [03_CuboNotificador.cs](https://github.com/guillrl1/P04_II_Delegados_Eventos/blob/main/Scripts/03_CuboNotificador.cs)
- [03_Perseguidor.cs](https://github.com/guillrl1/P04_II_Delegados_Eventos/blob/main/Scripts/03_Perseguidor.cs)

### Demostración
![Demostración Ejercicio 3](https://github.com/guillrl1/P04_II_Delegados_Eventos/blob/main/P4_gifs/03.gif)

---

## Ejercicio 4: Teletransporte y Orientación por aproximación

### Explicación del ejercicio
Colocamos un objeto de referencia en la escena. Cuando el cubo se aproxima a él, los humanoides del grupo `Tipo1` se teletransportan instantáneamente a un escudo objetivo fijado de antemano, mientras que los humanoides del grupo `Tipo2` rotan en el sitio para orientarse hacia un objeto específico de la escena.

### Cómo lo hemos resuelto y funciones clave
- **Detección de proximidad con Trigger (`04_ReferenciaNotificador.cs`)**: En lugar de calcular distancias en cada fotograma, añadimos un `Collider` amplio con `Is Trigger` activado al objeto de referencia. Cuando el cubo entra en el radio (`OnTriggerEnter`), se dispara el evento `OnCuboCerca`.
- **Teletransporte y rotación (`04_HumanoideEjercicio4.cs`)**:
  - Para el `Tipo1`, asignamos directamente `transform.position = escudoObjetivo.position`.
  - Para el `Tipo2`, calculamos el punto objetivo conservando la altura `transform.position.y` del Lich y aplicamos `transform.LookAt(puntoAMirar)`.

### Scripts del ejercicio
- [04_ReferenciaNotificador.cs](https://github.com/guillrl1/P04_II_Delegados_Eventos/blob/main/Scripts/04_ReferenciaNotificador.cs)
- [04_HumanoideEjercicio4.cs](https://github.com/guillrl1/P04_II_Delegados_Eventos/blob/main/Scripts/04_HumanoideEjercicio4.cs)

### Demostración
![Demostración Ejercicio 4](https://github.com/guillrl1/P04_II_Delegados_Eventos/blob/main/P4_gifs/04.gif)

---

## Ejercicio 5: Recolección de escudos y puntuación en Consola

### Explicación del ejercicio
Implementamos la mecánica de recolección de escudos con el cubo. Al tocar un escudo de `Tipo1` el jugador suma 5 puntos, y al tocar uno de `Tipo2` suma 10 puntos, destruyendo el escudo recolectado y mostrando el total acumulado por consola.

### Cómo lo hemos resuelto y funciones clave
- **Gestión de colisiones y destrucción (`05_RecolectorEscudosConsola.cs`)**: Detectamos con qué objeto ha chocado el cubo mediante `collision.collider` / `other`. Si tiene el tag `Escudo1` o `Escudo2`, incrementamos la variable `puntuacion`, mostramos el mensaje con `Debug.Log()` y eliminamos el escudo de la escena con `Destroy(otro.gameObject)`.

### Scripts del ejercicio
- [05_RecolectorEscudosConsola.cs](https://github.com/guillrl1/P04_II_Delegados_Eventos/blob/main/Scripts/05_RecolectorEscudosConsola.cs)

### Demostración
![Demostración Ejercicio 5](https://github.com/guillrl1/P04_II_Delegados_Eventos/blob/main/P4_gifs/05.gif)

---

## Ejercicio 6: Interfaz de usuario (Canvas) para la puntuación

### Explicación del ejercicio
Partiendo del ejercicio anterior, creamos una interfaz gráfica con un objeto `Canvas` y un componente de texto que muestra en tiempo real la puntuación que va obteniendo el cubo al recoger escudos.

### Cómo lo hemos resuelto y funciones clave
- **Actualización de UI con TextMeshPro (`06_PuntuacionUI.cs`)**: Importamos la librería `TMPro` y enlazamos una variable pública `TMP_Text textoPuntuacion`. Cada vez que se recoge un escudo, llamamos a `ActualizarTextoUI()` para modificar `textoPuntuacion.text = "Puntuación: " + puntuacion`.

### Scripts del ejercicio
- [06_PuntuacionUI.cs](https://github.com/guillrl1/P04_II_Delegados_Eventos/blob/main/Scripts/06_PuntuacionUI.cs)

### Demostración
![Demostración Ejercicio 6](https://github.com/guillrl1/P04_II_Delegados_Eventos/blob/main/P4_gifs/06.gif)

---

## Ejercicio 7 y 8: Sistema de Recompensas cada 100 puntos y Escena del Prototipo

### Explicación del ejercicio
Añadimos una mecánica por la cual cada vez que el jugador alcanza o supera un múltiplo de 100 puntos recibe una recompensa (aumento de velocidad del cubo) y se muestra un aviso visual en la UI. Finalmente, integramos todos los elementos (terreno delimitado, objeto de referencia central, humanoides de ambos grupos y escudos recolectables) en una escena de prototipo completa.

### Cómo lo hemos resuelto y funciones clave
- **Control de metas y aviso temporal (`07_SistemaRecompensas.cs`)**: Guardamos una variable `siguienteMeta = 100`. En el método `SumarPuntos()`, cuando `puntuacion >= siguienteMeta`, sumamos `100` a la siguiente meta, incrementamos la `velocidad` del jugador y activamos el objeto de texto de recompensa con `textoRecompensa.gameObject.SetActive(true)`.
- **`Invoke` y `CancelInvoke`**: Para que el aviso de recompensa no se quede fijo en pantalla para siempre, programamos su ocultación automática a los 3 segundos usando `Invoke(nameof(OcultarRecompensa), 3.0f)`.

### Scripts del ejercicio
- [07_SistemaRecompensas.cs](https://github.com/guillrl1/P04_II_Delegados_Eventos/blob/main/Scripts/07_SistemaRecompensas.cs)

### Demostración
![Demostración Ejercicio 7 y 8](https://github.com/guillrl1/P04_II_Delegados_Eventos/blob/main/P4_gifs/07.gif)
