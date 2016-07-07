#version 400

layout(location = 0) in vec3 inputPosition;
layout(location = 1) in vec3 inputNormal;
layout(location = 2) in vec3 inputBinormal;
layout(location = 3) in vec3 inputTangent;
layout(location = 4) in vec2 inputTexCoord;
layout(location = 5) in vec2 inputTexCoord2;


out vec2 texCoord;
out vec2 texCoord2;
out vec3 normal;
out vec3 binormal;
out vec3 tangent;
out vec3 verticePosition;


uniform mat4 viewMatrix;
uniform mat4 projMatrix;
uniform mat4 worldMatrix;


void main()
{
	gl_Position =  worldMatrix * projMatrix * viewMatrix * vec4(inputPosition,1.0);


	texCoord = inputTexCoord;
	texCoord2 = inputTexCoord2;
	normal = inputNormal;
	binormal = inputBinormal;
	tangent = inputTangent;

	verticePosition = normalize(inputPosition);
}