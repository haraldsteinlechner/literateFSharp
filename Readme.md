Tool for extracting code and comments out of literate fsharp code.

e.g.
flit example.fsl -o example.fs example.tex
extracts all literate comments into the tex file, while all fsharp code goes to example.fs.

literate fsharp code uses standard fsharp comments extended with underscore, e.g.
(*_ \textbf{Hello World} _*) 

Build:
run "bootstrap.cmd build" or bootstrap.sh build"