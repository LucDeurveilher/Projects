#version 120

uniform sampler2D texture;

void main()
{
	vec2 texcoord = gl_TexCoord[0].xy;
	vec4 pixel = texture2D(texture, texcoord);

    gl_FragColor = vec4(1-pixel.rgb,pixel.a) ;
}

