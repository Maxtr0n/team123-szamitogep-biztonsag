cmake_minimum_required(VERSION 3.19)
project(parser)

set(CMAKE_CXX_STANDARD 14)

add_executable(parser main.cpp parserClasses.h gif.h json.hpp)
