
# CAFF parser
## Fordítás
A forráskód fordításához mi a [CLion](https://www.jetbrains.com/clion/) programot, és az abban található [CMake](https://cmake.org/) eszközt használtuk.
## Használat
A lefordított parser program használatához 4 argumentum megadása szükséges:

|#|Argumentum |Leírás|
|--|--|--|
|1.|`--if`|Kapcsoló, amely azt jelzi, hogy a következő argumentum az `[input file path]`.  |
|2.|`[input file path]`|Útvonal a feldolgozandó `.caff` fájlhoz.|
|3.|`--of`|Kapcsoló, amely azt jelzi, hogy a következő argumentum az `[output file name]`.  |
|4.|`[output file name]`|Útvonal és fájlnév a kimeneti fájlokhoz, kiterjesztés **nélkül**.|


Az 1. és 3. argumentum sorrendje felcserélhető (ekkor nyilvánvalóan a 2-4. is cserélődik).
A program futtatása után a megadott kimeneti célhelyre két fájl generálódik: egy `.gif` az eredeti fájl animációjával, illetve egy `.json`, amely a `.caff`-ban tárolt metaadatokat tartalmazza további feldolgozás céljából.

## Felhasznált külső könyvtárak
A parser megírásához a következő külső forráskódokat használtuk fel:
- a GIF készítéshez [Charlie Tangora `gif.h` könyvtárát](https://github.com/charlietangora/gif-h/blob/master/gif.h)
- a JSON sorosításhoz [Niels Lohmann `json.hpp` könyvtárát](https://github.com/nlohmann/json/blob/develop/single_include/nlohmann/json.hpp)