#version 330

in vec2 texCoord;
in vec3 normal;
in vec3 verticePosition;
in vec3 binormal;
in vec3 tangent;


out vec4 colorOut;

uniform vec3 lightPosition;
uniform vec3 lightAmbientColor;
uniform vec3 lightDiffuseColor;

uniform sampler2D textureSample;
uniform sampler2D textureSample2;
uniform sampler2D textureSample3;
uniform sampler2D textureSample4;




void main() 
{
	vec4 color  = texture(textureSample, texCoord/4);
	vec4 color2 = texture(textureSample2, texCoord);
	vec4 color3 = texture(textureSample3, texCoord/4);
	vec4 color4 = normalize(2*texture(textureSample4, texCoord) - 1);

	color*=color;
	color2*= color2;
	color3*=color3;

	float slopex = sqrt(1 - normal.x * normal.x);
	float slopey = sqrt(1 - normal.y * normal.y);
	float slopez = sqrt(1 - normal.z * normal.z);

	vec4 colorMap = vec4(slopex, slopey , slopez,1);
	colorMap = colorMap;

	vec4 result;

	if(slopey < 0.5)
		result = mix(color, color2, colorMap.y/0.5);
	if(slopey >= 0.5 && slopey <0.8)
		result = mix(color, color3, colorMap.y);
	else
		result =  mix(color, color3, colorMap.y);

	vec3 Normal = normalize(normal);

	float diffuseN = max(dot(normalize(lightPosition - verticePosition), Normal),0.0);

    colorOut = result * vec4(lightAmbientColor,1.0) * vec4(0.2,0.2,0.2,0.0) + diffuseN * vec4(0.5,0.5,0.5,0.0) * result;
}