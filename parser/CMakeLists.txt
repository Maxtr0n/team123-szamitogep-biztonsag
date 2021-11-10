cmake_minimum_required(VERSION 3.19)
project(parser)

set(CMAKE_CXX_STANDARD 14)

add_executable(parser main.cpp parserClasses.hpp gif.h json.hpp parserClasses.cpp)
