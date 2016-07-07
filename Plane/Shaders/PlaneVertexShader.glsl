#version 330

layout(location = 0) in vec3 inputPosition;
layout(location = 1) in vec3 inputNormal;
layout(location = 4) in vec2 inputTexCoord;

out vec2 texCoord;
out vec3 normal;
out vec4 position;
out vec4 ClipPlaneReflection;
out vec4 ClipPlaneRefraction;

uniform mat4 viewMatrix;
uniform mat4 projMatrix;
uniform mat4 worldMatrix;

uniform vec4 clipPlaneReflection;
uniform vec4 clipPlaneRefraction;

void main()
{
	gl_Position = worldMatrix * projMatrix * viewMatrix * vec4(inputPosition,1.0);
	texCoord = inputTexCoord;
	normal = normalize(inputNormal);
	position =  normalize(vec4(inputPosition,1.0));
	
	ClipPlaneReflection = clipPlaneReflection;
	ClipPlaneRefraction = clipPlaneRefraction;

}