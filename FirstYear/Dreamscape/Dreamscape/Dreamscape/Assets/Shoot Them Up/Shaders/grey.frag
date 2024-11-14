#version 120

uniform sampler2D texture;

void main()
{
	vec2 texcoord = gl_TexCoord[0].xy;
	vec4 pixel = texture2D(texture, texcoord);
	
	float greyLevel = 0.299 * pixel.r + 0.587 * pixel.g + 0.114 * pixel.b;
	
    gl_FragColor = vec4(vec3(greyLevel), pixel.a);
}

