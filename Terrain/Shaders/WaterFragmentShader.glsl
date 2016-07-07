#version 330

in vec4 texCoord;
in vec3 normal;
in vec3 binormal;
in vec3 tangent;

in vec4 Position;
in vec4 verticePosition;
in vec3 camera;
in vec3 LightPosition;
in float Time;

out vec4 colourOut;

uniform vec4 lightAmbientColor;
uniform vec4 lightDiffuseColor;
uniform vec4 lightSpecularColor;

uniform sampler2D textureSample1;
uniform sampler2D textureSample2;
uniform sampler2D textureSample3;
uniform sampler2D textureSample5;

uniform float fresnelParameter;

uniform int diffuse;

const vec4 WATER_COLOR = vec4(0.0078,0.3176,0.7,1);
const vec4 SKY_COLOR = vec4(0.1,0.1,0.1,1);
const vec4 WATER_DEEPCOLOR = vec4(0.0039,0.196,0.3,1);


void main() 
{	
	vec4 textureProj = texCoord;

	textureProj.xyz /= textureProj.w;
	textureProj.x =  0.5f*textureProj.x + 0.5f;
	textureProj.y = -0.5f*textureProj.y + 0.5f;
	textureProj.z =  0.1f/ textureProj.z;

	vec3 toEye  = normalize(verticePosition.xyz - camera);
	vec3 Eye    = normalize(camera);
	
	vec4 bump1 = 2 * texture(textureSample5, Position.xz/16 - 0.005*Time) - 1;
	vec4 bump2 = 2 * texture(textureSample5, Position.xz/32 - 0.05*Time) - 1;

	vec3 Tangent = normalize(tangent);
	vec3 Binormal = normalize(binormal);
	vec3 Normal = normalize(1.5*normal + (bump1.xyz + bump2.xyz)/2);

	if(dot(normal, -toEye) < 0)
		Normal = reflect(Normal, -toEye);
	

	vec3 Direction = -normalize(LightPosition -vec3(548, 200, 320));

	vec3 halfVector = normalize(Direction + toEye);

	float s;

	if(diffuse == 1)
		s =  max(dot(-Direction, Normal),0.0);
	else
		s=1;

	float t;
	if(s > 0)
		t = max(pow(dot(reflect(Normal, Direction), toEye), 125), 0) * 100;
	else 
		t=0;
	
	
	
	float fresnel =  fresnelParameter + (1-fresnelParameter) * pow(1 - dot(toEye, halfVector),5);
	fresnel = min(1, fresnel - 0.007 * Eye.y);
	
	vec4 reflection = texture(textureSample2, textureProj.xy - textureProj.z * (Normal.xz));
	vec4 refraction = texture(textureSample1, textureProj.xy + textureProj.z * (Normal.xz));

	vec4 foam = texture(textureSample3, verticePosition.xz/512  - 0.005*Time);
	vec4 foam2 = texture(textureSample3, verticePosition.xz/128 - 0.05*Time);

	

	foam.a = clamp(verticePosition.y/30, 0, 1);

	reflection = mix(reflection, foam * foam * foam2 * foam2, foam.a);
	
	vec4 waterColor = mix(WATER_DEEPCOLOR, WATER_COLOR, clamp(Position.y,0,1));

	vec4 colorMaterial = waterColor*mix(refraction, reflection , 1 - fresnel);

	if(Eye.y < Position.y)
		colorMaterial = refraction * waterColor;
	
    vec4 result =  lightAmbientColor *  colorMaterial +  colorMaterial * lightDiffuseColor * s  + t * lightSpecularColor  + SKY_COLOR;

	colourOut.xyz = result.xyz;
	colourOut.a = 1;
}

