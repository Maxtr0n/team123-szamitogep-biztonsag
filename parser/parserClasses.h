#ifndef PARSER_PARSERCLASSES_H
#define PARSER_PARSERCLASSES_H

#include <cstdint>
#include <vector>
#include <fstream>
#include <sstream>

using namespace std;

class CiffHeader {
public:
    /**	Magic: 4 ASCII character spelling 'CIFF'*/
    char magic[4];

	/** Header size: 8-byte-long integer, its value is the size of the header
	(all fields included), i.e. the first header_size number of bytes in the
	file make up the whole header.*/
    uint64_t headerSize{};

	/** Content size: 8-byte long integer, its value is the size of the image
	pixels located at the end of the file. Its value must be width*heigth*3.*/
    uint64_t contentSize{};

	/** Width: 8-byte-long integer giving the width of the image. Its value can
	be zero, however, no pixels must be present in the file in that case. */
	uint64_t width{};

	/** Height: 8-byte-long integer giving the height of the image. Its value can
	be zero, however, no pixels must be present in the file in that case. */
	uint64_t height{};

	/** Caption: Variable-length ASCII encoded string ending with \n. It is the
	caption of the image. As \n is a special character for the file format, the
	caption cannot contains this character.*/
	string caption{};

	/** Tags: Variable number of variable-length ASCII encoded strings, each
	separated by \0 characters. The strings themselves must not be multiline.
	There must be a \0 character after the last tag as well.*/
    vector<string> tags{};

    void read(ifstream &ifstream) {
        ifstream.read(magic, 4);
        ifstream.read(reinterpret_cast<char *>(&headerSize), 8);
        ifstream.read(reinterpret_cast<char *>(&contentSize), 8);
        ifstream.read(reinterpret_cast<char *>(&width), 8);
        ifstream.read(reinterpret_cast<char *>(&height), 8);
        getline(ifstream,caption, '\n');
        size_t tags_length = headerSize - (36 + caption.size());
        string buffer;
        ifstream.read(reinterpret_cast<char *>(&buffer), tags_length);


    }
};

class CiffContent {
public:
    /** The header is followed by the actual pixels of the image in RGB format, with
    each component taking up 1 byte. This part of the CIFF file must contain
    exactly content_size number of bytes.*/
    vector<char> pixels{};

    void read(ifstream &ifstream) {

    }
};


class Ciff {
public:
    CiffHeader header;
    CiffContent content;

    void read(ifstream &ifstream) {
        header.read(ifstream);
        content.read(ifstream);
    }
};

class CaffBlock {
public:
    /** ID: 1-byte number which identifies the type of the block:
            0x1 - header
            0x2 - credits
            0x3 - animation*/
    unsigned int id{};

    /** Length: 8-byte-long integer giving the length of the block. */
    uint64_t length{};

    /** Data: This section is length bytes long and contain the data of the block.*/

};

class CaffHeader {
public:
    /** Magic: 4 ASCII character spelling 'CAFF'*/
    char magic[4];

    /** Header size: 8-byte-long integer, its value is the size of the header
    (all fields included).*/
    uint64_t headerSize{};

    /** Number of animated CIFFs: 8-byte long integer, gives the number of CIFF
            animation blocks in the CAFF file.*/
    uint64_t numOfCiffs{};

    void read(ifstream &is) {
        is.read(reinterpret_cast<char *>(&magic), 4);
        is.read(reinterpret_cast<char *>(&headerSize), 8);
        is.read(reinterpret_cast<char *>(&numOfCiffs), 8);
    }
};

class CaffCredits {
    /** Creation date and time: the year, month, day, hour and minute of the CAFF
	file's creation:
		Y - year (2 bytes)
		M - month (1 byte)
		D - day (1 byte)
		h - hour (1 byte)
		m - minute (1 byte) */
    char year[2]{};
    char month{};
    char day{};
    char hour{};
    char minute{};

	/** Length of creator: 8-byte-long integer, the length of the field
	specifying the creator. */
    uint64_t lenghtOfCreator{};

	/** Creator: Variable-length ASCII string, the creator of the CAFF file.*/
	string creator{};
public:
    void read(ifstream &is) {
        is.read(year, 2);
        is.read(&month, 1);
        is.read(&day, 1);
        is.read(&hour, 1);
        is.read(&minute, 1);

        is.read(reinterpret_cast<char *>(&lenghtOfCreator), 8);
        is.read(reinterpret_cast<char *>(&creator), lenghtOfCreator);
    }
};

class CaffAnimation {
public:
    /** Duration: 8-byte-long integer, miliseconds for which the CIFF image must
            be displayed during animation.*/
    uint64_t duration{};

    /* CIFF: the image to be displayed in CIFF format.*/
    Ciff ciff{};

    void read(ifstream &is){
        is.read(reinterpret_cast<char *>(&duration), 8);
        ciff.read(is);
    }
};

class Caff {
public:
    CaffBlock headerInfo{};
    CaffBlock creditsInfo{};
    CaffBlock animationInfo{};

    CaffHeader header{};
    CaffCredits credits{};

    vector<CaffAnimation> animation{};


};



#endif //PARSER_PARSERCLASSES_H
