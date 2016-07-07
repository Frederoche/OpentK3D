#version 400

in vec2 texCoord;
in vec3 inputNormal;
in vec4 position;

out vec4 colourOut;

in vec4 ClipPlaneReflection;
in vec4 ClipPlaneRefraction;

uniform sampler2D textureSample;

void main() 
{
	vec4 tex = texture(textureSample, texCoord);
	if(dot(ClipPlaneReflection.xyz, position.xyz) > 0 && ClipPlaneReflection.y == -1)
		discard;
	 if(dot(ClipPlaneRefraction.xyz, position.xyz) > 0 && ClipPlaneReflection.y == 1)
		discard;

	else	
		colourOut =  tex*tex;
}