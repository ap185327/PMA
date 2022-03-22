<p align="center">
  <img src="content/pma.png?raw=true">
</p>

# Introduction
Pali Morphological Analyzer (PMA) is designed for morphological and etymological analysis of single wordforms in the Pali language.

The input data for analysis is the wordform of the Pali language, its morphological parameters may or may not be known. The result of the analysis is a morphological and etymological analysis of the input wordform and its parts. It should be noted that a wordform can have several solutions. Each part of a wordform can also have several solutions. Finally, the result of the analysis is presented as a multi-level tree-like hierarchy of all possible solutions.
<p align="center">
  <img src="content/treeview.png?raw=true">
</p>

Simplified description of the algorithm:
1. searching for a word form in a morphological dictionary
2. if the word form is found, then the analysis ends
3. if the word form is not found, then the wordform analysis using grammatical rules and sandhi rules

As a result of using the rules, the wordform is divided into two parts (left and right wordforms). For each of the parts, steps 1-3 are repeated.
The analysis continues until all parts of the wordform are found in the morphological dictionary.

# Get Started
There are two types of applications: WinForms and WebServer. For Windows, you can use either Winforms or WebServer. For other OS, you need to use only WebServer.
## WinForms
1. Download and extract [latest WinForms release](https://github.com/ap185327/PMA/releases/download/v0.9.1-beta/pma-winforms.zip)
2. Download [database of dictionaries and rules](https://drive.google.com/file/d/19TFYaiz8qNcKpeGUBtk_jAq8sRme4P1s/view?usp=sharing)
3. Put database file in the application root directory
4. Run <b>PMA.WinForms.exe</b>

<p align="center">
  <img src="content/winforms_demo.gif?raw=true">
</p>

## WebServer
1. Download and extract [latest WebServer release](https://github.com/ap185327/PMA/releases/download/v0.9.1-beta/pma-blazor.zip)
2. Download [database of dictionaries and rules](https://drive.google.com/file/d/19TFYaiz8qNcKpeGUBtk_jAq8sRme4P1s/view?usp=sharing)
3. Put database file in the application root directory.
4. Download and install [.NET 6.0 Runtime](https://dotnet.microsoft.com/en-us/download)
5. In application root directory run command: <b>dotnet PMA.Blazor.dll</b>
6. Open <b>localhost:5000</b> or <b>localhost:5001</b> in web browser

<p align="center">
  <img src="content/blazor_screen01.png?raw=true">
</p>
