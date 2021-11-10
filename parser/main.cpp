#include <iostream>
#include "parserClasses.hpp"
#include "gif.h"
#include "json.hpp"

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
    Caff caff;
    try {
        ifstream file;
        file.open(argv[argIn], ios::in | ios::binary);
        if (!file.is_open()) {
            cerr << "Couldn't open given file." << endl;
            exit(3);
        }

        while (!file.eof()) {
            CaffBlock read;
            file.read((char *) (&read.id), 1);
            file.read(reinterpret_cast<char *>(&read.length), 8);

            switch (read.id) {
                case '\x01':
                    caff.header.read(file);
                    if (caff.header.headerSize != read.length)
                        throw CaffFormatErrorException("Invalid header type CaffBlock format.");
                    break;
                case '\x02':
                    caff.credits.read(file);
                    if (read.length != 14 + caff.credits.lenghtOfCreator)
                        throw CaffFormatErrorException("Invalid credit type CaffBlock format.");
                    break;
                case '\x03':
                    caff.animation.emplace_back();
                    caff.animation.at(caff.animation.size() - 1).read(file);
                    if (read.length != caff.animation.at(caff.animation.size() - 1).ciff.header.headerSize +
                                       caff.animation.at(caff.animation.size() - 1).ciff.header.contentSize + 8)
                        throw CaffFormatErrorException("Invalid animation type CaffBlock format.");
                    break;
                default:
                    if(!file.eof())
                        throw CaffFormatErrorException("Invalid CaffBlock id.");
                    break;
            }
        }

        if (caff.header.numOfCiffs != caff.animation.size())
            throw CaffFormatErrorException("Invalid number of CIFF files.");
    }catch (CaffFormatErrorException &e){
        cout<<"Error while trying to parse CAFF file: "<<e.message()<<'\n';
        exit(4);
    }
    /** ---------------- OPEN & READ END ---------------- */

    /** ---------------- GENERATE GIF ---------------- */

    uint32_t width = caff.animation.at(0).ciff.header.width;
    uint32_t height = caff.animation.at(0).ciff.header.height;

    auto fileName = fileOut + string(".gif");
    GifWriter g;
    GifBegin(&g, fileName.c_str(), width, height, 1);
    for (auto &f : caff.animation) {
        vector<uint8_t> pixelToGif{};
        size_t n = 0;

        for (auto &p: f.ciff.content.pixels) {
            pixelToGif.push_back(p);
            n++;
            if (n == 3) {
                pixelToGif.push_back(255);
                n = 0;
            }
        }
        GifWriteFrame(&g, pixelToGif.data(), width, height, f.duration/10);
    }
    GifEnd(&g);

    /** ---------------- GENERATE GIF END ---------------- */

    /** ---------------- JSON ---------------- */

    nlohmann::json j;

    j["width"] = width;
    j["height"] = height;

    j["creation_date"]["year"] = caff.credits.year;
    j["creation_date"]["month"] = caff.credits.month;
    j["creation_date"]["day"] = caff.credits.day;
    j["creation_date"]["hour"] = caff.credits.hour;
    j["creation_date"]["minute"] = caff.credits.minute;

    j["creator"] = caff.credits.creator;

    int fc = 0;
    for (auto &f: caff.animation) {
        j["frames"][fc]["counter"] = fc;
        j["frames"][fc]["caption"] = f.ciff.header.caption;
        j["frames"][fc]["tags"] = f.ciff.header.tags;
        fc++;
    }

    ofstream jsonOut(fileOut + ".json");
    if (!jsonOut.is_open()) {
        cerr << "Couldn't create " << fileOut << ".json file." << endl;
        exit(6);
    }
    jsonOut << j;
    jsonOut.close();

    /** ---------------- JSON END ---------------- */

    cout << "Parsing ended successfully." << endl;

    return 0;
}
