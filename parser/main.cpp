#include <iostream>
#include <fstream>
#include "parserClasses.h"
using namespace std;

int main() {

    // parser.exe --if asdf/ldldf.caff --of asdf/XY.gif
    // <img src="asdf/XY.gif">

    Caff c;

    ifstream file;
    file.open("../examples/2.caff", ios::in | ios::binary);
    if (!file.is_open()) {
        exit(1);
    }
    while (!file.eof()) {
        CaffBlock read;
        file.read((char *)(&read.id), 1);
        file.read(reinterpret_cast<char *>(&read.length), 8);

        switch (read.id) {
            case '\x01':
                //c.header
                c.header.read(file);
                break;
            case '\x02':
                c.credits.read(file);
                break;
            case '\x03':
                c.animation.emplace_back();
                c.animation.at(c.animation.size()-1).read(file);
                break;
            default:
                break;
        }
    }

    return 0;
}
