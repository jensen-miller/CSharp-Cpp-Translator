cmake_minimum_required(VERSION 3.8)

project("BlinkSample" CXX)


add_executable(blink_sample_program "${PROJECT_SOURCE_DIR}/src/program.cpp")
target_include_directories(blink_sample_program PUBLIC "${PROJECT_SOURCE_DIR}/../../dotnet-core-sdk-cpp/include")