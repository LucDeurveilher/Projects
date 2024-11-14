#version 120

uniform sampler2D texture;
uniform vec2 screenSize;
uniform vec2 pos;
uniform float intensity;

void main()
{
	// récupère le pixel dans la texture
	vec2 texcoord = gl_TexCoord[0].xy;
	texcoord.x *= 1.7777;

	vec4 pixel = texture2D(texture, texcoord);

	vec2 objectPos = pos;
	objectPos.x *= 1.7777;

	float originalDistance = distance(texcoord.xy, objectPos/screenSize);

	float resultat = pow(1 - originalDistance, 8) * intensity;

	gl_FragColor = vec4(vec3(resultat * 0.5), 1);
}