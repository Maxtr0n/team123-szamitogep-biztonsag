#ifndef PARSER_PARSERCLASSES_H
#define PARSER_PARSERCLASSES_H

#include <cstdint>
#include <vector>
#include <fstream>
#include <sstream>

using namespace std;

bool magicMatch(char a[], char b[]){
    for(int i =0; i<4; i++){
        if(a[i] != b[i])
            return false;
    }
    return true;
}

struct CaffFormatErrorException : public std::exception
{
    string msg;

    CaffFormatErrorException(const char* _msg){
        msg = _msg;
    }
    string message () const throw ()
    {
        return msg;
    }
};

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
        char check[] = {'C','I','F','F'};
        if(!magicMatch(magic,check))
            throw CaffFormatErrorException("Invalid CIFF file format.");
        ifstream.read(reinterpret_cast<char *>(&headerSize), 8);
        ifstream.read(reinterpret_cast<char *>(&contentSize), 8);
        ifstream.read(reinterpret_cast<char *>(&width), 8);
        ifstream.read(reinterpret_cast<char *>(&height), 8);
        getline(ifstream,caption, '\n');
        size_t tags_length = headerSize - (36 + caption.size());
        uint64_t buffer_size = 0;
        while(buffer_size + 1<tags_length){
            try{
                string temp;
                getline(ifstream, temp, '\0');
                buffer_size += temp.size() + 1;
                tags.push_back(temp);
            }
            catch(exception e) {
                throw CaffFormatErrorException("Invalid CIFF tags format.");
            }
        }

        for(auto a:tags){
            if(a.find('\n') != std::string::npos)
                throw CaffFormatErrorException("Multiline tags in CIFF.");
        }
        if(tags.size()>headerSize)
            throw CaffFormatErrorException("Invalid CIFF tags format.");

        if(headerSize != 4+8+8+8+8+caption.length()+tags_length)
            throw CaffFormatErrorException("Invalid header_size in CIFF header.");
        if(contentSize != width*height*3)
            throw CaffFormatErrorException("Invalid content_size in CIFF header.");
    }
};

class CiffContent {
public:
    /** The header is followed by the actual pixels of the image in RGB format, with
    each component taking up 1 byte. This part of the CIFF file must contain
    exactly content_size number of bytes.*/
    vector<uint8_t> pixels{};

    void read(ifstream &ifstream, uint64_t contentSize) {
        for(int i=0; i<contentSize; i++){
            uint8_t temp;
            ifstream.read(reinterpret_cast<char *>(&temp), 1);
            pixels.push_back(temp);
        }
    }
};


class Ciff {
public:
    CiffHeader header;
    CiffContent content;

    void read(ifstream &ifstream) {
        header.read(ifstream);
        content.read(ifstream, header.contentSize);
        if(content.pixels.size()!=header.contentSize)
            throw CaffFormatErrorException("Invalid content format in CIFF file.");
        if((header.width == 0 || header.height == 0) && content.pixels.size()!=0 )
            throw CaffFormatErrorException("Invalid pixel data in CIFF file.");
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
        char check[] = {'C','A','F','F'};
        if(!magicMatch(magic,check))
            throw CaffFormatErrorException("Invalid CAFF file format.");
        is.read(reinterpret_cast<char *>(&headerSize), 8);
        if(headerSize!= 4+8+8)
            throw CaffFormatErrorException("Invalid CAFF header size.");
        is.read(reinterpret_cast<char *>(&numOfCiffs), 8);
    }
};

class CaffCredits {
public:
    /** Creation date and time: the year, month, day, hour and minute of the CAFF
	file's creation:
		Y - year (2 bytes)
		M - month (1 byte)
		D - day (1 byte)
		h - hour (1 byte)
		m - minute (1 byte) */
    uint64_t year{};
    uint64_t month{};
    uint64_t day{};
    uint64_t hour{};
    uint64_t minute{};

	/** Length of creator: 8-byte-long integer, the length of the field
	specifying the creator. */
    uint64_t lenghtOfCreator{};

	/** Creator: Variable-length ASCII string, the creator of the CAFF file.*/
	string creator{};
public:
    void read(ifstream &is) {
        is.read(reinterpret_cast<char *>(&year), 2);
        is.read(reinterpret_cast<char *>(&month), 1);
        is.read(reinterpret_cast<char *>(&day), 1);
        is.read(reinterpret_cast<char *>(&hour), 1);
        is.read(reinterpret_cast<char *>(&minute), 1);

        is.read(reinterpret_cast<char *>(&lenghtOfCreator), 8);
        if (lenghtOfCreator > 0) {
            vector<char> buffer(lenghtOfCreator);
            is.read(&buffer.front(), lenghtOfCreator);
            creator = std::string(buffer.begin(), buffer.end());
        } else {
            creator = "";
        }
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
    CaffHeader header{};
    CaffCredits credits{};

    vector<CaffAnimation> animation{};


};



#endif //PARSER_PARSERCLASSES_H
