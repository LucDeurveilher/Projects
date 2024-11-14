#version 120

uniform sampler2D texture;

void main()
{
	vec2 texcoord = gl_TexCoord[0].xy;
	vec4 pixel = texture2D(texture, texcoord);

	float dist = distance(vec2(0.5,0.5),texcoord);

	dist = dist < 0.4 ? 255 : 0;

    gl_FragColor = vec4(vec3(dist),pixel.a);
}

