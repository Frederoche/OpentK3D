#version 330

layout(location = 0) in vec3 inputPosition;
layout(location = 1) in vec3 inputNormal;
layout(location = 4) in vec2 inputTexCoord;

out vec4 texCoord;
out vec3 normal;
out vec3 binormal;
out vec3 tangent;

out vec4 Position;
out vec4 verticePosition;
out vec3 camera;
out vec3 LightPosition;
out float Time;

uniform mat4 viewMatrix;
uniform mat4 projMatrix;
uniform mat4 worldMatrix;
uniform vec3 cameraPosition;
uniform vec3 lightPosition;
uniform int calm;
uniform float time;
uniform float noise;
uniform float stormParameter[5];

uniform sampler1D textureSample4;

float Pi = 3.1415;
float g = 9.81;
float alpha = 0.0081;
float betha = 0.74;
float WindV = 4;

float Dispersion(vec2 wavenumber)
{
	return sqrt(g*length(wavenumber));
}

float Phase(vec2 waveNumber, vec4 initialPosition)
{
	return - Dispersion(waveNumber)*time + dot(waveNumber, initialPosition.xz);
}

float Amplitude(vec2 wavenumber) //Pierson Moskovitz distribution
{
	return sqrt(alpha*pow(g,2)/pow(Dispersion(wavenumber),5)*exp(-betha*pow((WindV/(g*20))/(Dispersion(wavenumber)),4)));
}

vec4 GrestnerWave(vec4 initialPosition, vec2 wave)
{
	vec4 result = vec4(0,0,0,1);

	result.x =  wave.x/length(wave) * Amplitude(wave) *  sin(Phase(wave, initialPosition));	
	result.z =  wave.y/length(wave) * Amplitude(wave) *  sin(Phase(wave, initialPosition));

	result.y = Amplitude(wave) *cos(Phase(wave, initialPosition));
	return result;
}

vec4 dWavedx(vec4 initialPosition, vec2 wave)
{
	vec4 result = vec4(0,0,0,1);

	result.x =  wave.x * wave.x/length(wave) * Amplitude(wave) * cos(Phase(wave, initialPosition));
	result.z =  wave.y * wave.x/length(wave) * Amplitude(wave) * cos(Phase(wave, initialPosition));	

	result.y = - Amplitude(wave) * wave.x  * sin(Phase(wave, initialPosition));
	return result;
}

vec4 dWavedz(vec4 initialPosition, vec2 wave)
{
	vec4 result = vec4(0,0,0,1);

	result.x =  wave.x* wave.y/length(wave) * Amplitude(wave) * cos(Phase(wave, initialPosition));
	result.z =  wave.y* wave.y/length(wave) * Amplitude(wave) * cos(Phase(wave, initialPosition));	

	result.y = - Amplitude(wave) *  wave.y * sin(Phase(wave, initialPosition));
	return result;
}


void main()
{
	vec4 pos =  vec4(inputPosition,1.0);
	vec4 pos2 = vec4(inputPosition,1.0);


	vec4 sum = vec4(0,0,0,0);

	vec4 sumTangent = vec4(0,0,0,0);
	vec4 sumBinormal = vec4(0,0,0,0);

	vec2 wave0 = vec2(0.1,0.1);
	vec2 wave1 = vec2(0.2,0.2);
	vec2 wave2 = vec2(0.3, -0.3);
	vec2 wave3 = vec2(0.4, -0.4);
	vec2 wave4 = vec2(-0.5,  0.005);
	vec2 wave5 = vec2( 0.6, 0.6);
	vec2 wave6 = vec2(0.3, -0.7);
	vec2 wave7 = vec2(0.4, -0.8);
	vec2 wave8 = vec2( -0.9,0.2);
	vec2 wave9 = vec2(-1,1);
	vec2 wave10 = vec2(0.01,-0.01);
	vec2 wave11 = vec2(-0.3,0.2);
	vec2 wave12 = vec2(-0.4,0.25);
	vec2 wave13 = vec2(0.45,0.5);
	vec2 wave14 = vec2(0.45, -0.25);
	vec2 wave15 = vec2(0.35, 0.24);
	vec2 wave16 = 10*vec2(0.17, -0.18);
	vec2 wave17 = vec2(3, 2);
	vec2 wave18 = vec2(0.05, 0.125);
	vec2 wave19 = vec2(0.121,0.121);
	vec2 wave20 = vec2(-0.15,0.135);
	vec2 wave21 = vec2(4.14,4.13);
	vec2 wave22 = vec2(-0.07,0.04);
	vec2 wave23 = vec2(-0.015,0.012);
	vec2 wave24 = vec2(0.015, 0.015);

	if(calm == 1)
	{
		for(int i=0; i <4; i++)
		{
			vec2 waveNumbers= vec2(texelFetch(textureSample4,i,0).x, texelFetch(textureSample4,i,0).y);
			sum+= GrestnerWave(pos, waveNumbers);
			sumTangent+= dWavedx(pos, waveNumbers);
			sumBinormal+= dWavedz(pos, waveNumbers);
		}
	}

	if(calm == 0)
	{
		sum= GrestnerWave(pos, wave0);
		sum+= GrestnerWave(pos, wave1);
		sum+= GrestnerWave(pos, wave2);
		sum+= GrestnerWave(pos, wave3);
		sum+= GrestnerWave(pos, wave4);
		sum+= GrestnerWave(pos, wave5);
		sum+= GrestnerWave(pos, wave6);
		sum+= GrestnerWave(pos, wave7);
		sum+= stormParameter[0]*GrestnerWave(pos, wave8);
		sum+= GrestnerWave(pos, wave9);
		sum+= GrestnerWave(pos, wave10);
		sum+= GrestnerWave(pos, wave11);
		sum+= GrestnerWave(pos, wave12);
		sum+= GrestnerWave(pos, wave13);
		sum+= stormParameter[1]*GrestnerWave(pos, wave14);
		sum+= GrestnerWave(pos, wave15);
		sum+= stormParameter[2]*GrestnerWave(pos, wave16);
		sum+= GrestnerWave(pos, wave17);
		sum+= GrestnerWave(pos, wave18);
		sum+= stormParameter[3]*GrestnerWave(pos, wave19);
		sum+= GrestnerWave(pos, wave20);
		sum+= GrestnerWave(pos, wave21);
		sum+= GrestnerWave(pos, wave22);
		sum+= GrestnerWave(pos, wave23);
		sum+= stormParameter[4]*GrestnerWave(pos, wave24);

		sumTangent= dWavedx(pos, wave0);
		sumTangent+= dWavedx(pos, wave1);
		sumTangent+= dWavedx(pos, wave2);
		sumTangent+= dWavedx(pos, wave3);
		sumTangent+= dWavedx(pos, wave4);
		sumTangent+= dWavedx(pos, wave5);
		sumTangent+= dWavedx(pos, wave6);
		sumTangent+= dWavedx(pos, wave7);
		sumTangent+= dWavedx(pos, wave8);
		sumTangent+= dWavedx(pos, wave9);
		sumTangent+= dWavedx(pos, wave10);
		sumTangent+= dWavedx(pos, wave11);
		sumTangent+= dWavedx(pos, wave12);
		sumTangent+= dWavedx(pos, wave13);
		sumTangent+=stormParameter[0]*dWavedx(pos, wave14);
		sumTangent+= dWavedx(pos, wave15);
		sumTangent+= stormParameter[1]*dWavedx(pos, wave16);
		sumTangent+= dWavedx(pos, wave17);
		sumTangent+= dWavedx(pos, wave18);
		sumTangent+= stormParameter[2]*dWavedx(pos, wave19);
		sumTangent+= dWavedx(pos, wave20);
		sumTangent+= dWavedx(pos, wave21);
		sumTangent+= dWavedx(pos, wave22);
		sumTangent+= dWavedx(pos, wave23);
		sumTangent+= stormParameter[3]*dWavedx(pos, wave24);

		sumBinormal= dWavedz(pos, wave0);
		sumBinormal+= dWavedz(pos, wave1);
		sumBinormal+= dWavedz(pos, wave2);
		sumBinormal+= dWavedz(pos, wave3);
		sumBinormal+= dWavedz(pos, wave4);
		sumBinormal+= dWavedz(pos, wave5);
		sumBinormal+= dWavedz(pos, wave6);
		sumBinormal+= dWavedz(pos, wave7);
		sumBinormal+= dWavedz(pos, wave8);
		sumBinormal+= dWavedz(pos, wave9);
		sumBinormal+= dWavedz(pos, wave10);
		sumBinormal+= dWavedz(pos, wave11);
		sumBinormal+= dWavedz(pos, wave12);
		sumBinormal+= dWavedz(pos, wave13);
		sumBinormal+= stormParameter[0]*dWavedz(pos, wave14);
		sumBinormal+= dWavedz(pos, wave15);
		sumBinormal+= stormParameter[1]*dWavedz(pos, wave16);
		sumBinormal+= dWavedz(pos, wave17);
		sumBinormal+= dWavedz(pos, wave18);
		sumBinormal+= stormParameter[2]*dWavedz(pos, wave19);
		sumBinormal+= dWavedz(pos, wave20);
		sumBinormal+= dWavedz(pos, wave21);
		sumBinormal+= dWavedz(pos, wave22);
		sumBinormal+= dWavedz(pos, wave23);
		sumBinormal+= stormParameter[3]*dWavedz(pos, wave24);
	}

	pos2.xz = pos.xz - sum.xz;
	pos2.y  = sum.y;
		
	normal = (vec4(cross(vec3(1 + sumTangent.x, sumTangent.y, sumTangent.z), vec3(sumBinormal.x, sumBinormal.y, 1 + sumBinormal.z)),1)).xyz;
	tangent  = vec3(1 + sumTangent.x, sumTangent.y, sumTangent.z);
	binormal = vec3(sumBinormal.x, sumBinormal.y, 1 + sumBinormal.z);

	Position = projMatrix * viewMatrix * pos2;
	texCoord = Position;

	verticePosition = pos2;

	camera = cameraPosition.xyz;
	Time = time;

	LightPosition =  (vec4(lightPosition,1)).xyz;

	gl_Position = Position;
}
