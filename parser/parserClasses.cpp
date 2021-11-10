
#include "parserClasses.hpp"

bool magicMatch(const char a[], const char b[]){
    for(int i =0; i<4; i++){
        if(a[i] != b[i])
            return false;
    }
    return true;
}

void CiffHeader::read(ifstream &ifstream) {
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
        catch(exception& e) {
            throw CaffFormatErrorException("Invalid CIFF tags format.");
        }
    }

    for(auto& a:tags){
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

void CiffContent::read(ifstream &ifstream, uint64_t contentSize) {
    for(int i=0; i<contentSize; i++){
        uint8_t temp;
        ifstream.read(reinterpret_cast<char *>(&temp), 1);
        pixels.push_back(temp);
    }
}

void Ciff::read(ifstream &ifstream) {
    header.read(ifstream);
    content.read(ifstream, header.contentSize);
    if(content.pixels.size()!=header.contentSize)
        throw CaffFormatErrorException("Invalid content format in CIFF file.");
    if((header.width == 0 || header.height == 0) && !content.pixels.empty() )
        throw CaffFormatErrorException("Invalid pixel data in CIFF file.");
}

void CaffHeader::read(ifstream &is) {
    is.read(reinterpret_cast<char *>(&magic), 4);
    char check[] = {'C','A','F','F'};
    if(!magicMatch(magic,check))
        throw CaffFormatErrorException("Invalid CAFF file format.");
    is.read(reinterpret_cast<char *>(&headerSize), 8);
    if(headerSize!= 4+8+8)
        throw CaffFormatErrorException("Invalid CAFF header size.");
    is.read(reinterpret_cast<char *>(&numOfCiffs), 8);
}

void CaffCredits::read(ifstream &is) {
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

void CaffAnimation::read(ifstream &is) {
    is.read(reinterpret_cast<char *>(&duration), 8);
    ciff.read(is);
}
