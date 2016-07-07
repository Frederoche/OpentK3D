#version 330

layout(location = 0) in vec3 inputPosition;
layout(location = 1) in vec3 inputNormal;

out vec4 position;
out vec3 normal;

uniform mat4 viewMatrix;
uniform mat4 projMatrix;
uniform mat4 worldMatrix;

void main()
{
	gl_Position = worldMatrix * projMatrix * viewMatrix * vec4(inputPosition,1.0);
	position =  vec4(inputPosition,1.0);
	normal = normalize(inputNormal);
}