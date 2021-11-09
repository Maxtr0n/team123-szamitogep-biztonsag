#include <iostream>
#include <fstream>
#include "parserClasses.h"
#include "bitmap.h"
using namespace std;

int main(int argc, char* argv[]) {

    /** ---------------- ARGUMENT CHECK ---------------- */

    if (argc != 5) {
        cerr << "The program must have 4 arguments:" << endl;
        cerr << "--if\t\t\tMarking the input path, following by [input file]" << endl;
        cerr << "[input file]\t\tPath to the file to be opened" << endl;
        cerr << "--of\t\t\tMarking the output path, following by [output file]" << endl;
        cerr << "[output file]\t\tPath to the output file, without extension." << endl;
        exit(-1);
    }

    Caff caff;

    int argIn;
    int argOut;
    if (strcmp(argv[1], "--if") != 0 &&
        strcmp(argv[3], "--if") != 0) {
        cerr << "First or third argument must be '--if', following the input file." << endl;
        exit(1);
    } else {
        if (strcmp(argv[1], "--if") == 0) {
            argIn = 2;
            argOut = 4;
        } else {
            argIn = 4;
            argOut = 2;
        }
    }
    if (strcmp(argv[argOut-1], "--of") != 0) {
        cerr << "First or third argument must be '--of', following the output filename (without extension)." << endl;
        exit(2);
    }
    string fileOut(argv[argOut]); // Output path

    /** ---------------- ARGUMENT CHECK END ---------------- */

    /** ---------------- OPEN & READ ---------------- */
    ifstream file;
    file.open(argv[argIn], ios::in | ios::binary);
    if (!file.is_open()) {
        cerr << "Couldn't open given file." << endl;
        exit(3);
    }
    while (!file.eof()) {
        CaffBlock read;
        file.read((char *)(&read.id), 1);
        file.read(reinterpret_cast<char *>(&read.length), 8);

        switch (read.id) {
            case '\x01':
                caff.header.read(file);
                break;
            case '\x02':
                caff.credits.read(file);
                break;
            case '\x03':
                caff.animation.emplace_back();
                caff.animation.at(caff.animation.size() - 1).read(file);
                break;
            default:
                break;
        }
    }

    /** ---------------- OPEN & READ END ---------------- */

    genBitmap(caff);

    return 0;
}
