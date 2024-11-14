#version 120

uniform sampler2D texture;
uniform float saturation;

void main()
{
	// récupère le pixel dans la texture
	vec2 texcoord = gl_TexCoord[0].xy;

	vec4 pixel = texture2D(texture, texcoord);

	gl_FragColor.rgb = mix(vec3(dot(pixel.rgb, vec3(0.299, 0.587, 0.114))), pixel.rgb, saturation);
	gl_FragColor.a = pixel.a;
}