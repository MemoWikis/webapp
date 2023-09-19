export default defineNitroPlugin((nitroApp) => {
    nitroApp.hooks.hook('render:html', (html: any, { event }) => {      
        if (html.head) {
            //To fix FOUC (Flush of unstyled content) on topic page make _id[xyz].css preload instead of prefetch
            //Disable from time to time to check if still necessary
            html.head = html.head.map((headString: string) => {
                return headString.replace(/<link[^>]+prefetch[^>]+_id[^>]+\.css"/g, match => {
                    return match.replace('prefetch', 'preload');
                });
            });
        }
        return html;
    });
});
